import { BrowserRouter, Routes, Route } from 'react-router-dom'
import Home from './pages/Home'
import Authors from './pages/Authors'
import SignIn from './pages/SignIn'
import SignUp from './pages/SignUp'
import Dashboard from './pages/Dashboard'
import Header from './components/Header'
import Footer from './components/Footer'

export default function App() {
  return (
    <BrowserRouter>
      <Header />
      <Routes>
        <Route path='/' element={<Home/>} />
        <Route path='/authors' element={<Authors/>} />
        <Route path='/sign-in' element={<SignIn/>} />
        <Route path='/sign-up' element={<SignUp/>} />
        <Route path='/dashboards' element={<Dashboard/>} />
      </Routes>
      <Footer />
    </BrowserRouter>
  )
}
