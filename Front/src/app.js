const http = require('http');
const fs = require('fs');
const path = require('path');

const PORT = 3000;

function renderizarPagina(pageName, res) {
    const baseDir = path.join(__dirname, 'pages', pageName);
    const baseComponents = path.join(__dirname, 'components');

    const main = fs.readFileSync(path.join(__dirname, 'main.html'), 'utf-8');
    const navbarTemplate = fs.readFileSync(path.join(baseComponents, 'navbar', 'navbar.html'), 'utf-8');
    const navbarJs = fs.readFileSync(path.join(baseComponents, 'navbar', 'navbar.js'), 'utf-8');
    const footbar = fs.readFileSync(path.join(baseComponents, 'footbar', 'footbar.html'), 'utf-8');
    const loginModal = fs.readFileSync(path.join(baseComponents, 'login', 'login.html'), 'utf-8');
    const pageHtml = fs.readFileSync(path.join(baseDir, `${pageName}.html`), 'utf-8');
    const pageCss = `<link rel="stylesheet" href="/pages/${pageName}/${pageName}.css">`;
    const pageJs = `<script src="/pages/${pageName}/${pageName}.js"></script>`;

    let html = main
        .replace('{{navbar}}', navbarTemplate)
        .replace('{{navbarJs}}', navbarJs)
        .replace('{{footbar}}', footbar)
        .replace('{{loginModal}}', loginModal)
        .replace('{{pageHtml}}', pageHtml)
        .replace('{{pageCss}}', pageCss)
        .replace('{{pageJs}}', pageJs);

    res.writeHead(200, { 'Content-Type': 'text/html' });
    res.end(html, 'utf-8');
}

function tratarRotaDinamica(req, res) {
    if (req.url === '/' || req.url === '/inicio') {
        renderizarPagina('inicio', res);
        return true;
    } else if (req.url === '/analises') {
        renderizarPagina('analises', res);
        return true;
    }
    else if (req.url === '/salvos') {
        renderizarPagina('salvos', res);
        return true;
    }
    return false;
}

function disponibilizarArquivos(req) {
    if (req.url.startsWith('/pages/')) {
        return path.join(__dirname, 'pages', req.url.replace('/pages/', ''));
    } else if (req.url.startsWith('/components/')) {
        return path.join(__dirname, 'components', req.url.replace('/components/', ''));
    }
    else if (req.url.startsWith('/publics/')) {
        return path.join(__dirname, 'publics', req.url.replace('/publics/', ''));
    }
    else if (req.url.startsWith('/styles/')) {
        return path.join(__dirname, 'styles', req.url.replace('/styles/', ''));
    }
    else {
        return null;
    }
}

const server = http.createServer((req, res) => {
    let filePath;

    if (tratarRotaDinamica(req, res)) return;

    filePath = disponibilizarArquivos(req);

    if (!filePath) {
        res.writeHead(404, { 'Content-Type': 'text/plain' });
        res.end('Página não encontrada');
        return;
    }

    const extname = String(path.extname(filePath)).toLowerCase();
    const mimeTypes = {
        '.html': 'text/html',
        '.js': 'text/javascript',
        '.css': 'text/css',
        '.png': 'image/png'
    };

    const contentType = mimeTypes[extname] || 'application/octet-stream';

    fs.readFile(filePath, (error, content) => {
        if (error) {
            res.writeHead(404, { 'Content-Type': 'text/plain' });
            res.end('Pagina nao encontrada');
        } else {
            res.writeHead(200, { 'Content-Type': contentType });
            res.end(content, 'utf-8');
        }
    });
});

server.listen(PORT, () => {
    console.log(`Servidor rodando em http://localhost:${PORT}`);
});