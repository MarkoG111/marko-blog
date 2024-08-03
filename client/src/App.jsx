import { BrowserRouter, Routes, Route } from 'react-router-dom'
import Home from './pages/Home'
import Authors from './pages/Authors'
import SignIn from './pages/SignIn'
import SignUp from './pages/SignUp'
import Dashboard from './pages/Dashboard'
import Header from './components/Header'
import Footer from './components/Footer'
import PrivateRoute from './components/PrivateRoute'
import OnlyRolePrivateRoute from './components/OnlyRolePrivateRoute'
import CreatePost from './pages/CreatePost'
import UpdatePost from './pages/UpdatePost'
import PostPage from './pages/PostPage'
import UserPage from './pages/UserPage'
import ScrollToTop from './components/ScrollToTop'
import PostsPage from './pages/PostsPage'
import CategoryPage from './pages/CategoryPage'
import UserCommentPage from './pages/UserCommentPage'

export default function App() {
  return (
    <BrowserRouter>
      <ScrollToTop />
      <Header />
      <Routes>
        <Route path='/' element={<Home />} />
        <Route path='/authors' element={<Authors />} />
        <Route path='/posts' element={<PostsPage />} />
        <Route path='/sign-in' element={<SignIn />} />
        <Route path='/sign-up' element={<SignUp />} />
        <Route element={<PrivateRoute />}>
          <Route path='/dashboard' element={<Dashboard />} />
        </Route>
        <Route element={<OnlyRolePrivateRoute />}>
          <Route path='/create-post' element={<CreatePost />} />
          <Route path='/update-post/:postId' element={<UpdatePost />} />
        </Route>

        <Route path='/post/:id' element={<PostPage />} />
        <Route path='/user/:id' element={<UserPage />} />
        <Route path='/category/:id' element={<CategoryPage />} />
        <Route path='/comment/:id' element={<UserCommentPage />} />
      </Routes>
      <Footer />
    </BrowserRouter>
  )
}
