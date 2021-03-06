/* eslint-disable no-console */
import express from 'express'
import next from 'next'
import jwt from 'jsonwebtoken'
import favicon from 'serve-favicon'
import path from 'path'
import fetch from 'isomorphic-unfetch'
import cookieParser from 'cookie-parser'
import { parse } from 'url'
import config from './config'

const __dirname = path.resolve()
const dev = process.env.NODE_ENV !== 'production'
const app = next({ dev })

const authSites = [
    { name: 'github', url: process.env.GITHUB_AUTH_URL || null }
]

const createOrUpdateUser = (issuer, code) => {
    const options = {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
        }
    }
    return fetch(`${config.api.identity.uri}/users/issuer/${issuer}/code/${code}`, options)
        .then(r => r.json())
}

const getUserToken = async (id) => {
    const options = {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
        }
    }
    const res = await fetch(`${config.api.identity.uri}users/${id}/token`, options)
    if (!res.ok) {
        return
    }
    return res.json()
}

app
    .prepare()
    .then(() => {
        const server = express()
        const port = dev ? 3000 : 80

        server.use(favicon(path.join(__dirname, 'static', 'favicon.ico')))
        server.use(cookieParser())

        server.get('/authentication', async (req, res) => {
            const issuer = req.query.issuer
            const code= req.query.code

            if (!issuer || !code) {
                return res.sendStatus(404)
            }

            const id = await createOrUpdateUser(issuer, code)
            const token = await getUserToken(id)

            if (!token) {
                return res.sendStatus(400)
            }

            res.cookie('accessToken', token, {
                httpOnly: true,
                secure: !dev,
                sameSite: true,
            })

            const parsedUrl = parse(req.url, true)
            const { query } = parsedUrl

            app.render(req, res, '/authentication', { ...query, sites: authSites, accessToken: token })
        })

        server.get('/logout', async (req, res) => {
            const parsedUrl = parse(req.url, true)
            const { query } = parsedUrl
            res.clearCookie('accessToken')
            return app.render(req, res, '/logout', query)
        })

        server.get('*', async (req, res) => {
            const accessToken = req.cookies.accessToken
            const parsedUrl = parse(req.url, true)
            const { pathname, query } = parsedUrl
            const isResourceRequest = pathname.startsWith('/_next') || pathname.startsWith('/static')

            if (!accessToken) {
                return app.render(req, res, pathname, { ...query, sites: authSites, accessToken: null })
            }

            if (isResourceRequest) {
                return app.render(req, res, pathname, { ...query, sites: authSites, accessToken: accessToken })
            }

            const payload = jwt.decode(accessToken)

            if (!payload || !payload.id) {
                res.clearCookie('accessToken')
                return app.render(req, res, pathname, { ...query, sites: authSites, accessToken: null })
            }

            const newAccessToken = await getUserToken(payload.id)

            if (!newAccessToken) {
                res.clearCookie('accessToken')
                return app.render(req, res, pathname, { ...query, sites: authSites, accessToken: null })
            }

            return app.render(req, res, pathname, { ...query, sites: authSites, accessToken: newAccessToken })
        })

        server.listen(port, (err) => {
            if (err) throw err
            console.log(`> Ready on http://localhost:${port}`)
        })
    })
    .catch((ex) => {
        console.error(ex.stack)
        process.exit(1)
    })