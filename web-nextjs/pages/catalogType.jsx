import React from 'react'
import PropTypes from 'prop-types'
import { connect } from 'react-redux'
import Container from '../components/layout/Container'
import { loadCatalogTypes } from '../actions/catalogType.actions'
import CatalogTypeList from '../components/page/catalogType/CatalogTypeList'
import { storeUser } from '../actions/identity.actions'

class CatalogType extends React.Component {
    constructor(props) {
        super(props)
    }

    static async getInitialProps({ ctx: { store, query } }) {
        store.dispatch(loadCatalogTypes())
        if (query.user) {
            store.dispatch(storeUser(query.user))
        }
    }

    static propTypes = {
        dispatch: PropTypes.func.isRequired
    }

    componentDidMount() {
        this.props.dispatch(loadCatalogTypes())
    }

    render() {
        return (
            <Container title='Catalog Type' header={<div/>}>
                <CatalogTypeList/>
            </Container>
        )
    }
}

export default connect()(CatalogType)
export { CatalogType }