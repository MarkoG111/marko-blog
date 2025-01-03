import { useEffect, useState } from "react"
import { useParams } from "react-router-dom"
import { HiDocumentText, HiOutlineMail } from 'react-icons/hi'
import { FaRegCommentDots, FaUsers } from 'react-icons/fa'
import { RiUserFollowLine, RiUserUnfollowFill } from "react-icons/ri"
import { FaUserPlus } from "react-icons/fa6"
import { Button } from "flowbite-react"
import { useError } from "../contexts/ErrorContext"
import { handleApiError } from "../utils/handleApiUtils"

export default function UserPage() {
  const { id } = useParams()

  const [user, setUser] = useState(null)
  const [isFollowing, setIsFollowing] = useState(false)

  const { showError } = useError()

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const response = await fetch(`/users/${id}`, {
          method: "GET"
        })

        const data = await response.json()

        if (response.ok) {
          setUser(data)

          const token = localStorage.getItem("token")
          if (token) {
            const followResponse = await fetch(`/followers/${id}/check`, {
              method: "GET",
              headers: {
                "Authorization": `Bearer ${token}`
              }
            })

            const followData = await followResponse.json()
            setIsFollowing(followData.isFollowing)
          }
        } else {
          await handleApiError(response, showError)
        }
      } catch (error) {
        showError(error.message)
      }
    }

    fetchUser()
  }, [id, showError])

  const handleFollow = async (id) => {
    try {
      const token = localStorage.getItem("token")
      if (!token) {
        throw new Error("Token not found")
      }

      const response = await fetch(`/followers`, {
        method: "POST",
        headers: {
          "Authorization": `Bearer ${token}`,
          "Content-Type": "Application/json"
        },
        body: JSON.stringify({ IdFollowing: id })
      })

      if (response.ok) {
        setIsFollowing(true)

        setUser((prevUser) => ({
          ...prevUser,
          followersCount: prevUser.followersCount + 1,
        }))
      } else {
        await handleApiError(response, showError)
      }
    } catch (error) {
      showError(error.message)
    }
  }

  const handleUnfollow = async (id) => {
    try {
      const token = localStorage.getItem("token")
      if (!token) {
        throw new Error("Token not found")
      }

      const response = await fetch(`/followers/${id}/unfollow`, {
        method: "DELETE",
        headers: {
          "Authorization": `Bearer ${token}`,
        }
      })

      if (response.ok) {
        setIsFollowing(false)

        setUser((prevUser) => ({
          ...prevUser,
          followersCount: prevUser.followersCount - 1,
        }))
      } else {
        await handleApiError(response, showError)
      }
    } catch (error) {
      showError(error.message)
    }
  }

  const latestPost = user && user.userPosts[0]
  const otherPosts = user && user.userPosts.slice(1)

  return (
    <main className="max-w-6xl mx-auto min-h-screen p-3">
      <div>
        <header className="dark:bg-gray-800 border border-teal-500 rounded-lg text-center mt-12">
          <div className="relative mt-negative">
            <span className="inline-block relative align-middle overflow-hidden w-[90px] h-[90px]">
              <img src={user && user.profilePicture} alt="profilePicture" className="rounded-full inline-block align-bottom w-full h-full" />
            </span>
          </div>

          <h1 className="text-3xl p-3 text-center font-serif max-w-2xl mx-auto lg:text-4xl">{user && user.firstName} {user && user.lastName}</h1>

          <div className="flex -mt-28 gap-x-2 ml-6">
            <FaUsers className="h-6 w-6 text-gray-400 dark:text-gray-200 mb-3" /> <span>{user && user.followersCount} {user && user.followersCount == 1 ? 'follower' : 'followers'}</span>
            <RiUserFollowLine className="h-6 w-6 text-gray-400 dark:text-gray-200 mb-3 ml-4" /> <span>{user && user.followingCount} following</span>
          </div>
          <div className="flex -mt-9 gap-x-4 justify-end mr-8">
            {isFollowing ? (
              <Button
                type="button"
                gradientDuoTone="purpleToPink"
                onClick={() => handleUnfollow(user.id)}
              >
                <RiUserUnfollowFill className="h-6 w-6 text-white dark:text-gray-200 mr-4" />{" "}
                Unfollow
              </Button>
            ) : (
              <Button
                type="button"
                gradientDuoTone="purpleToPink"
                onClick={() => handleFollow(user.id)}
              >
                <FaUserPlus className="h-6 w-6 text-white dark:text-gray-200 mr-4" /> Follow
              </Button>
            )}
          </div>

          <div className="flex justify-center pb-5 pt-20">
            <span className="text-2xl pr-4 pt-1"><HiOutlineMail /></span>
            <p className="text-xl">{user && user.email}</p>
          </div>
          <div className="flex flex-col items-center pb-5">
            <HiDocumentText className="h-8 w-8 text-gray-400 dark:text-gray-200 mt-3" /> <span className="mb-3">{user && user.userPosts.length == 1 ? user && user.userPosts.length + ` post published` : user && user.userPosts.length + ` posts published`}</span>
            <FaRegCommentDots className="h-8 w-8 text-gray-400 dark:text-gray-200 my-3" /> <span>{user && user.userComments.length == 1 ? user && user.userComments.length + ` comment written` : user && user.userComments.length + ` comments written`}</span>
          </div>
        </header>
      </div>

      <div>
        <main className="mt-12">
          <div className="flex flex-wrap">
            {latestPost &&
              <>
                <div className="dark:bg-gray-800 border border-teal-500 rounded-lg mt-12 w-2/5 pb-8 mr-auto">
                  <div className="flex pt-4 pl-2">
                    <div>
                      <img src={user && user.profilePicture.startsWith('http') ? user && user.profilePicture : user && `users/images/${user.profilePicture}`} alt='user image' className='w-10 object-cover rounded-full mr-3' />
                    </div>
                    <div>
                      <span className="text-sm md:pl-3">{user && user.firstName + " " + user.lastName}</span> <br />
                      <span className="text-sm md:pl-3">{new Date(latestPost && latestPost.dateCreated).toLocaleDateString()}</span>
                    </div>
                  </div>
                  <div className="flex py-6 pl-2 md:pl-16">
                    <a href={`/post/${latestPost.id}`} className="hover:underline hover:text-teal-500 dark:hover:text-teal-300 transition duration-200 ease-in-out">
                      <h4 className="sm:text-xl md:text-2xl">{latestPost && latestPost.title}</h4>
                    </a>
                  </div>
                  <div className="text-left pl-2 md:pl-16">
                    <ul className="flex flex-wrap gap-y-6 gap-x-5">
                      {latestPost && latestPost.categories.map((category) => (
                        <li key={category.id} className="text-sm"><a href={`/category/${category.id}`} className="rounded p-2 dark:bg-gray-700 bg-gray-100 hover:underline hover:text-teal-500 dark:hover:text-teal-300 transition duration-200 ease-in-out">#{category.name}</a></li>
                      ))}
                    </ul>
                  </div>
                </div>
              </>
            }

            {user && user.userComments.length > 0 &&
              <>
                <div className="dark:bg-gray-800 border border-teal-500 rounded-lg text-left mt-12 w-1/2">
                  <h3 className="p-4 md:pt-3 md:pb-8 md:pl-8 sm:text-2xl md:text-3xl font-bold">Recent comments</h3>
                  <hr className="border-t-1 border-gray-300" />
                  {user && user.userComments.map((comment, index) => (
                    index < 2 &&
                    <a href={`/comment/${comment.id}`} key={comment.id}>
                      <div className="dark:hover:bg-slate-600 hover:bg-slate-100 hover:rounded-lg px-2 py-6 md:py-6 md:px-8">
                        <h4 className="md:text-2xl sm:text-xl">{comment.postTitle}</h4>
                        <div className="flex text-gray-400 justify-between">
                          <h3 className="text-sm">{comment.commentText}</h3>
                          <span className="text-sm pl-4">{new Date(comment && comment.createdAt).toLocaleDateString()}</span>
                        </div>
                      </div>
                      {index < 1 && <hr className="border-b-1 border-teal-500" />}
                    </a>
                  ))}
                </div>
              </>
            }

            {otherPosts && otherPosts.length > 0 &&
              <div className="dark:bg-gray-800 border border-teal-500 rounded-lg text-center my-12 w-full px-8 pb-8">
                {otherPosts && otherPosts.map((post, index) => (
                  <div className="flex flex-wrap" key={post.id}>
                    <div className="dark:bg-gray-800 border border-teal-500 rounded-lg mt-12 w-full pb-8 mr-auto">
                      <div className="flex pt-4 pl-2">
                        <div>
                          <img src={user && user.profilePicture.startsWith('http') ? user && user.profilePicture : user && `users/images/${user.profilePicture}`} alt='profilePicture' className='w-10 object-cover rounded-full' />
                        </div>
                        <div className="pl-4">
                          <span className="text-sm">{user && user.firstName + " " + user.lastName}</span> <br />
                          <span className="text-sm">{new Date(latestPost && latestPost.dateCreated).toLocaleDateString()}</span>
                        </div>
                      </div>
                      <div className="flex py-6 pl-16">
                        <a href={`/post/${post.id}`} className="hover:underline hover:text-teal-500 dark:hover:text-teal-300 transition duration-200 ease-in-out">
                          <h4 className="text-2xl">{post.title}</h4>
                        </a>
                      </div>
                      <div className="text-left pl-16">
                        <ul className="flex flex-wrap gap-y-6 gap-x-5">
                          {post && post.categories.map((category) => (
                            <li key={category.id} className="text-sm"><a href={`/category/${category.id}`} className="rounded p-2 dark:bg-gray-700 bg-gray-100 hover:underline hover:text-teal-500 dark:hover:text-teal-300 transition duration-200 ease-in-out">#{category.name}</a></li>
                          ))}
                        </ul>
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            }
          </div >
        </main>
      </div>
    </main>
  )
}