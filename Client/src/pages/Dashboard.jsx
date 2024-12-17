import { useEffect, useState } from 'react'
import { useLocation } from 'react-router-dom'
import DashSidebar from '../components/DashSidebar'
import DashProfile from '../components/DashProfile'
import DashPosts from '../components/DashPosts'
import DashUsers from '../components/DashUsers'
import RequestAuthorForm from '../components/RequestAuthorForm'
import DashAuthorRequests from '../components/DashAuthorRequests'
import DashComments from '../components/DashComments'
import AdminDashboard from '../components/AdminDashboard'
import FollowList from '../components/FollowList'
import UserDashPosts from '../components/UserDashPosts'

export default function Dashboard() {
  const location = useLocation()
  
  const [tab, setTab] = useState('')

  useEffect(() => {
    const urlParams = new URLSearchParams(location.search)
    const tabFromUrl = urlParams.get('tab')

    if (tabFromUrl) {
      setTab(tabFromUrl)
    }

  }, [location.search])

  return (
    <div className='min-h-screen flex flex-col md:flex-row'>
      <div className='md:w-56'>
        <DashSidebar />
      </div>

      {tab == 'dashboard' && < AdminDashboard />}
      {tab == 'profile' && <DashProfile />}
      {tab == 'comments' && <DashComments />}
      {tab == 'users' && <DashUsers />}
      {tab == 'posts' && <DashPosts />}
      {tab == 'authorRequests' && <DashAuthorRequests />}
      {tab == 'requestAuthorForm' && <RequestAuthorForm />}
      {tab == 'userPosts' && <UserDashPosts />}
      {tab == 'followers' && <FollowList isFollowersTab={true} />}
      {tab == 'following' && <FollowList isFollowersTab={false} />}
    </div>
  )
}
