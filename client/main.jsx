import React from 'react';
import ReactDOM from 'react-dom';
import ReactDOMServer from 'react-dom/server';

class UI {
    renderServer = () => {
        return ReactDOMServer.renderToString(
            <h1>Hello World!</h1>
        );
    }

    renderClient = () => {
        return ReactDOM.hydrate(
            <h1>Hello World!</h1>,
            document.getElementById('container')
        );
    }
}

exports.UI = UI;