import * as React from 'react';
import * as ReactDOM from 'react-dom';
import App from './App';
import registerServiceWorker from './registerServiceWorker';

ReactDOM.render(
    <React.StrictMode>
        <App />
    </React.StrictMode>,
    document.getElementById('root'));

registerServiceWorker();

export const hashCode = (s: string): number => {
    var hash = 0, i, chr;
    for (i = 0; i < s.length; i++) {
        chr = s.charCodeAt(i);
        hash = ((hash << 5) - hash) + chr + i;
        hash |= 0; // Convert to 32bit integer
    }
    return hash;
}