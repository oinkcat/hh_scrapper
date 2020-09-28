import './index.css';
import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';

if (!window.Symbol) {
    window.Symbol = {};
}

const rootElement = document.getElementById('root');

ReactDOM.render(<App />, rootElement);
