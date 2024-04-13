import { Link } from 'react-router-dom'

export default function Footer() {
  return (
    <footer className='bg-gradient-to-r from-indigo-500 via-purple-500 to-pink-500 rounded-lg p-20'>
      <ul className='flex items-center justify-center flex-wrap gap-6 mb-16 flex-col md:flex-row'>
        <li><Link to="/posts/categories/HTML">HTML</Link></li>
        <li><Link to="/posts/categories/CSS">CSS</Link></li>
        <li><Link to="/posts/categories/Sass">Sass</Link></li>
        <li><Link to="/posts/categories/Bootstrap">Bootstrap</Link></li>
        <li><Link to="/posts/categories/Material">Material Design</Link></li>
        <li><Link to="/posts/categories/JavaScript">JavaScript</Link></li>
        <li><Link to="/posts/categories/PHP">PHP</Link></li>
        <li><Link to="/posts/categories/Databases">Database Design</Link></li>
        <li><Link to="/posts/categories/MySQL">MySQL</Link></li>
        <li><Link to="/posts/categories/C#">C#</Link></li>
        <li><Link to="/posts/categories/ASP">ASP.NET</Link></li>
        <li><Link to="/posts/categories/Laravel">Laravel</Link></li>
        <li><Link to="/posts/categories/Vue">Vue</Link></li>
        <li><Link to="/posts/categories/React">React</Link></li>
        <li><Link to="/posts/categories/Angular">Angular</Link></li>
        <li><Link to="/posts/categories/TypeScript">TypeScript</Link></li>
        <li><Link to="/posts/categories/API">API</Link></li>
        <li><Link to="/posts/categories/SEO">SEO</Link></li>
        <li><Link to="/posts/categories/OOP">OOP</Link></li>
        <li><Link to="/posts/categories/Solid">SOLID Principles</Link></li>
        <li><Link to="/posts/categories/Patterns">Design Patterns</Link></li>
        <li><Link to="/posts/categories/Data">JSON/XML</Link></li>
        <li><Link to="/posts/categories/HTTP">HTTP/HTTPS</Link></li>
        <li><Link to="/posts/categories/Networking">Networking</Link></li>
        <li><Link to="/posts/categories/GIT">GIT</Link></li>
        <li><Link to="/posts/categories/Testing">Integration/Unit Testing</Link></li>
        <li><Link to="/posts/categories/Security">Web Security</Link></li>
        <li><Link to="/posts/categories/CI-CD">CI/CD</Link></li>
        <li><Link to="/posts/categories/Linux">Linux/Unix Commands</Link></li>
        <li><Link to="/posts/categories/Auth">Authentication/Authorization</Link></li>
      </ul>
    </footer>
  )
}
