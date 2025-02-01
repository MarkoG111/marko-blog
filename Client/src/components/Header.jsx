import { useContext, useEffect, useState } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { toggleTheme } from '../redux/theme/themeSlice'
import { signoutSuccess } from '../redux/user/userSlice'
import { Link, useLocation, useNavigate } from 'react-router-dom'
import { Avatar, Button, Dropdown, Navbar, TextInput } from 'flowbite-react'
import { AiOutlineSearch } from 'react-icons/ai'
import { FaMoon, FaSun, FaRegBell } from 'react-icons/fa'
import { NotificationsContext } from '../contexts/NotificationsContext'
import { useError } from '../contexts/ErrorContext'
import { getAvatarSrc } from '../utils/getAvatarSrc'

export default function Header() {
  const path = useLocation().pathname

  const navigate = useNavigate()

  const dispatch = useDispatch()

  const { theme } = useSelector((state) => state.theme)
  const { currentUser } = useSelector((state) => state.user)

  const [headerSearchTerm, setHeaderSearchTerm] = useState('')
  const [imageError, setImageError] = useState(false)

  const { notifications, hasNewNotifications } = useContext(NotificationsContext)

  const { showError } = useError()

  const unreadNotificationCount = notifications.filter((n) => !n.isRead).length

  const avatarSrc = getAvatarSrc(currentUser?.profilePicture, imageError)

  const isAuthor = currentUser && currentUser.roleName === 'Author'

  // Reset image error state when currentUser changes
  useEffect(() => {
    setImageError(false)
  }, [currentUser])

  const handleSignout = async () => {
    try {
      localStorage.removeItem("token")
      dispatch(signoutSuccess())
      navigate('/')
    } catch (error) {
      showError(error.message)
    }
  }

  const handleSearchSubmit = (e) => {
    e.preventDefault()
    navigate(`/posts?search=${headerSearchTerm}`)
  }

  return (
    <Navbar className='border-b-2'>
      {/* Logo */}
      <Link to='/' className='self-center whitespace-nowrap mt-4 md:mt-0 text-2xl md:text-xl font-semibold dark:text-white mx-auto'>
        <span className='px-2 py-1 bg-gradient-to-r from-indigo-500 via-purple-500 to-pink-500 rounded-lg text-white'>Marko&apos;s</span>Blog
      </Link>

      {/* Search Form */}
      <form onSubmit={handleSearchSubmit}>
        <TextInput type='text' placeholder='Search...' rightIcon={AiOutlineSearch} className='hidden lg:inline' value={headerSearchTerm} onChange={(e) => setHeaderSearchTerm(e.target.value)} />
      </form>

      {/* Right Side Controls */}
      <div className='flex gap-4 md:gap-2 md:order-2 mx-auto'>
        {currentUser && (currentUser.roleName === 'Admin' || currentUser.roleName === 'Author') && (
          <Link to='/create-post' className='mr-6'>
            <Button type="button" gradientDuoTone='purpleToPink' className="w-full size-18 mt-6 md:mt-2">Create Post</Button>
          </Link>
        )}
        {currentUser && (currentUser.roleName === 'Admin') && (
          <Link to='/create-category' className='mr-6'>
            <Button type="button" gradientDuoTone='purpleToPink' className="w-full size-18 mt-6 md:mt-2">Add Category</Button>
          </Link>
        )}

        {/* Theme Toggle Button */}
        <Button className='w-10 md:w-12 h-10 mt-6 md:mt-2 sm:block' color='gray' pill onClick={() => dispatch(toggleTheme())}>
          {theme === 'light' ? <FaMoon /> : <FaSun />}
        </Button>

        {/* Notifications Button */}
        {isAuthor && (
          <Link to='/notifications'>
            <Button className='w-10 md:w-12 h-10 mt-6 md:mt-2 rounded-full' color='gray'>
              <FaRegBell />
              {hasNewNotifications && (
                <span className='absolute bottom-5 left-6 inline-flex items-center justify-center px-2 py-1 text-xs font-bold leading-none text-red-100 bg-red-600 rounded-full'>{unreadNotificationCount}</span>
              )}
            </Button>
          </Link>
        )}

        {/* User Avatar or Sign In Button */}
        {currentUser ? (
          <Dropdown arrowIcon={false} inline label={
            <Avatar
              alt='user'
              img={avatarSrc}
              referrerPolicy="no-referrer"
              rounded
              className='mt-6 md:mt-2'
              onError={() => setImageError(true)}
            />
          }>
            <Dropdown.Header>
              <span className='block text-sm mb-2'>@{currentUser.username}</span>
              <span className='block text-sm font-medium truncate'>{currentUser.email}</span>
            </Dropdown.Header>
            <Link to={'/dashboard?tab=profile'}>
              <Dropdown.Item>Profile</Dropdown.Item>
            </Link>

            <Dropdown.Divider />
            <Dropdown.Item onClick={handleSignout}>Sign out</Dropdown.Item>
          </Dropdown>
        ) : (<Link to='/sign-in'>
          <Button gradientDuoTone='purpleToBlue' pill className='mt-6 md:mt-2'>
            Sign In
          </Button>
        </Link>)
        }

        {/* Navbar Toggle for Mobile */}
        <Navbar.Toggle className='mt-5' />
      </div>

      {/* Navbar Links */}
      <Navbar.Collapse className='md:ml-16'>
        <Navbar.Link active={path === '/'} as={'div'}>
          <Link to='/'>
            Home
          </Link>
        </Navbar.Link>
        <Navbar.Link active={path === '/authors'} as={'div'}>
          <Link to='/authors'>
            Authors
          </Link>
        </Navbar.Link>
        <Navbar.Link active={path === '/posts'} as={'div'}>
          <Link to='/posts'>
            Posts
          </Link>
        </Navbar.Link>
      </Navbar.Collapse>
    </Navbar >
  )
}
