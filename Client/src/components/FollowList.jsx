import { Link } from "react-router-dom";
import { useState, useEffect } from "react";
import { useSelector } from "react-redux";

/* eslint-disable react/prop-types */
export default function FollowList({ isFollowersTab }) {
  const [listUsers, setList] = useState([])
  const [currentPage, setCurrentPage] = useState(1)
  const [pageCount, setPageCount] = useState(1)

  const { currentUser } = useSelector((state) => state.user)

  useEffect(() => {
    const fetchList = async () => {
      try {
        const token = localStorage.getItem("token")
        if (!token) {
          throw new Error("Token not found")
        }

        const queryParams = new URLSearchParams({
          page: currentPage,
          perPage: 3,
        });

        const url = isFollowersTab ? `/api/Followers/${currentUser.id}/followers?${queryParams}` : `/api/Followers/${currentUser.id}/following?${queryParams}`

        const response = await fetch(url, {
          method: "GET",
          headers: {
            "Authorization": `Bearer ${token}`
          },
        })

        if (response.ok) {
          const data = await response.json()
          setList(data.items)
          setPageCount(data.pageCount)
        }
      } catch (error) {
        console.error("Error fetching followers:", error);
      }
    }

    fetchList()
  }, [isFollowersTab, currentPage])

  return (
    <div className='mx-auto'>
      <h1 className='mb-4 font-bold text-xl text-center my-7'>
        {isFollowersTab ? 'Followers' : 'Following'}
      </h1>

      {listUsers.length > 0 ? (
        <div className="flex flex-wrap justify-center gap-12">
          {listUsers.map((user) => {
            return (
              <div key={user.id} className="flex flex-col flex-2 min-w-[250px] items-center dark:bg-gray-800 border border-teal-500 pt-6 rounded-3xl text-center">
                <Link to={`/user/${user.id}`}>
                  <div className="flex flex-col items-center">
                    <img src={user.profilePicture} alt="profilePicture" className="w-32 object-cover rounded-full" />
                    <div className="py-8">
                      <p className="text-indigo-400 text-xl font-semibold">{user.firstName} {user.lastName}</p>
                      <p className="py-3">@{user.username}</p>
                      <p>{user.email}</p>
                    </div>
                  </div>
                </Link>
              </div>
            )
          })}
        </div>
      ) : (
        <p>No users found.</p>
      )
      }

      {
        pageCount > 1 && (
          <div className='flex justify-center space-x-2 mt-4'>
            {currentPage > 1 && (
              <button
                className='px-2 py-1 bg-gray-200 rounded'
                onClick={() => setCurrentPage((prev) => prev - 1)}
              >
                Previous
              </button>
            )}
            {currentPage < pageCount && (
              <button
                className='px-2 py-1 bg-gray-200 rounded'
                onClick={() => setCurrentPage((prev) => prev + 1)}
              >
                Next
              </button>
            )}
          </div>
        )
      }
    </div>
  );
}