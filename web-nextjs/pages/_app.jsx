import React from 'react'
import App, { Container } from 'next/app'
import { MuiThemeProvider } from '@material-ui/core/styles'
import CssBaseline from '@material-ui/core/CssBaseline'
import JssProvider from 'react-jss/lib/JssProvider'
import { Provider } from 'react-redux'
import withRedux from 'next-redux-wrapper'
import withReduxSaga from 'next-redux-saga'
import getPageContext from '../components/common/getPageContext'
import configureStore from '../store/store'
import { storeUser, storeAuthSites } from '../actions/identity.actions'

class MyApp extends App {
    constructor(props) {
        super(props)
        this.pageContext = getPageContext()
    }

    static async getInitialProps ({ Component, ctx }) {
        let pageProps = {}
        const query = ctx.query
        const store = ctx.store
        
        //check if run at server
        if (ctx.req) {
            store.dispatch(storeUser(query.user))
            store.dispatch(storeAuthSites(query.sites))
        }
    
        if (Component.getInitialProps) {
            pageProps = await Component.getInitialProps({ ctx })
        }
    
        return { pageProps }
    }

    componentDidMount() {
        // Remove the server-side injected CSS.
        const jssStyles = document.querySelector('#jss-server-side')
        if (jssStyles && jssStyles.parentNode) {
            jssStyles.parentNode.removeChild(jssStyles)
        }
    }

    render() {
        const { Component, pageProps, store } = this.props
        return (
            <Container>
                <Provider store={store}>
                    {/* Wrap every page in Jss and Theme providers */}
                    <JssProvider
                        registry={this.pageContext.sheetsRegistry}
                        generateClassName={this.pageContext.generateClassName}
                    >
                        {/* MuiThemeProvider makes the theme available down the React
                        tree thanks to React context. */}
                        <MuiThemeProvider
                            theme={this.pageContext.theme}
                            sheetsManager={this.pageContext.sheetsManager}
                        >
                            {/* CssBaseline kickstart an elegant, consistent, and simple baseline to build upon. */}
                            <CssBaseline />
                            {/* Pass pageContext to the _document though the renderPage enhancer
                                to render collected styles on server side. */}
                            <Component pageContext={this.pageContext} {...pageProps} />
                        </MuiThemeProvider>
                    </JssProvider>
                </Provider>
            </Container>
        )
    }
}

export default withRedux(configureStore)(withReduxSaga()(MyApp))