import { Sidebar, Button } from 'flowbite-react'
import { HiUser, HiArrowSmRight, HiDocumentText, HiOutlineUserGroup, HiOutlineUserAdd } from 'react-icons/hi'
import { useEffect, useState } from 'react';
import { Link, useLocation } from 'react-router-dom'
import { useDispatch, useSelector } from "react-redux"
import { signoutSuccess } from '../redux/user/userSlice';
import { RiPieChart2Fill } from "react-icons/ri";
import { FaRegComments } from "react-icons/fa";

export default function DashSidebar() {
  const location = useLocation();
  const [tab, setTab] = useState('');

  const [user, setUser] = useState('')

  const dispatch = useDispatch()

  const { currentUser } = useSelector(state => state.user)

  useEffect(() => {
    const urlParams = new URLSearchParams(location.search);
    const tabFromUrl = urlParams.get('tab');

    if (tabFromUrl) {
      setTab(tabFromUrl);
    }
  }, [location.search]);

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const repsonse = await fetch(`/api/Users/${currentUser.id}`, {
          method: "GET"
        })

        const data = await repsonse.json()

        if (repsonse.ok) {
          setUser(data)

          const token = localStorage.getItem("token")
          if (!token) {
            throw new Error("Token not found")
          }
        }
      } catch (error) {
        console.log(error)
      }
    }

    fetchUser()
  }, [currentUser.id])

  const handleSignout = async () => {
    try {
      localStorage.removeItem("token")
      dispatch(signoutSuccess())
    } catch (error) {
      console.log(error);
    }
  }

  const getRoleLabel = () => {
    if (currentUser.roleName === 'Admin') {
      return 'Admin'
    } else if (currentUser.roleName === 'Author') {
      return 'Author'
    } else {
      return 'User'
    }
  }

  return (
    <Sidebar className='w-full md:w-56'>
      <Sidebar.Items>
        <Sidebar.ItemGroup className='flex flex-col gap-1'>

          {currentUser.roleName === 'Admin' && (
            <Link to='/dashboard?tab=dashboard'>
              <Sidebar.Item active={tab == 'dashboard'} icon={RiPieChart2Fill} labelColor='dark' as='div'>
                Dashboard
              </Sidebar.Item>
            </Link>
          )}

          <Link to='/dashboard?tab=profile'>
            <Sidebar.Item active={tab == 'profile'} icon={HiUser} label={getRoleLabel()} labelColor='dark' as='div'>
              Profile
            </Sidebar.Item>
          </Link>

          {currentUser.roleName === 'User' && (
            <Link to='/dashboard?tab=requestAuthorForm'>
              <Button type="button" gradientDuoTone='purpleToPink' className="w-full">Request to Become an Author</Button>
            </Link>
          )}

          {currentUser.roleName === 'Author' && (
            <Link to='/dashboard?tab=userPosts'>
              <Sidebar.Item active={tab === 'userPosts'} as='div'>
                <div className='flex justify-between'>
                  <span>My Posts</span>
                  <span className="w-8 h-7 pt-1 text-center rounded-full dark:bg-cyan-600 bg-gray-600 text-white text-sm font-bold">{user.postsCount}</span>
                </div>
              </Sidebar.Item>
            </Link>
          )}

          {currentUser.roleName === 'Author' && (
            <Link to={'/dashboard?tab=followers'}>
              <Sidebar.Item active={tab === 'followers'} as='div'>
                <div className='flex justify-between'>
                  <span>Followers</span>
                  <span className="w-8 h-7 pt-1 text-center rounded-full dark:bg-cyan-600 bg-gray-600 text-white text-sm font-bold">{user.followersCount}</span>
                </div>
              </Sidebar.Item>
            </Link>
          )}

          {currentUser.roleName === 'Author' && (
            <Link to={'/dashboard?tab=following'}>
              <Sidebar.Item active={tab === 'following'} as='div'>
                <div className='flex justify-between'>
                  <span>Following</span>
                  <span className="w-8 h-7 pt-1 text-center rounded-full dark:bg-cyan-600 bg-gray-600 text-white text-sm font-bold">{user.followingCount}</span>
                </div>
              </Sidebar.Item>
            </Link>
          )}

          {currentUser.roleName === 'Admin' && (
            <Link to='/dashboard?tab=comments'>
              <Sidebar.Item active={tab === 'comments'} icon={FaRegComments} as='div'>
                Comments
              </Sidebar.Item>
            </Link>
          )}

          {currentUser.roleName === 'Admin' && (
            <Link to='/dashboard?tab=users'>
              <Sidebar.Item active={tab === 'users'} icon={HiOutlineUserGroup} as='div'>
                Users
              </Sidebar.Item>
            </Link>
          )}

          {currentUser.roleName === 'Admin' && (
            <Link to='/dashboard?tab=posts'>
              <Sidebar.Item active={tab === 'posts'} icon={HiDocumentText} as='div'>
                Posts
              </Sidebar.Item>
            </Link>
          )}

          {currentUser.roleName === 'Admin' && (
            <Link to='/dashboard?tab=authorRequests'>
              <Sidebar.Item active={tab === 'authorRequests'} icon={HiOutlineUserAdd} as='div'>
                Author Requests
              </Sidebar.Item>
            </Link>
          )}

          <Sidebar.Item icon={HiArrowSmRight} className='cursor-pointer' onClick={handleSignout}>
            Sign Out
          </Sidebar.Item>
        </Sidebar.ItemGroup>
      </Sidebar.Items>
    </Sidebar>
  )
}
