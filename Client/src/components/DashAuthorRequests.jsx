import { Table, Pagination, Button } from "flowbite-react";
import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { Link } from "react-router-dom";
import { HiOutlineClock, HiOutlineCheck, HiOutlineX } from 'react-icons/hi'
import { useError } from "../contexts/ErrorContext"

export default function DashAuthorRequests() {
  const { currentUser } = useSelector((state) => state.user)
  
  const [authorRequests, setAuthorRequests] = useState([])
  const [currentPage, setCurrentPage] = useState(1)
  const [pageCount, setPageCount] = useState(1)

  const { showError } = useError()

  useEffect(() => {
    const fetchAuthorRequests = async () => {
      try {
        const token = localStorage.getItem("token")
        if (!token) {
          throw new Error("Token not found")
        }

        const response = await fetch(`/api/AuthorRequests?page=${currentPage}`, {
          method: "GET",
          headers: {
            "Authorization": `Bearer ${token}`
          }
        })

        const data = await response.json()

        if (response.ok) {
          setAuthorRequests(data.items)
          setPageCount(data.pageCount)
        } else {
          throw new Error(data.message || "Failed to fetch requests")
        }
      } catch (error) {
        showError(error.message)
      }
    }

    fetchAuthorRequests()
  }, [currentPage, showError])

  const onPageChange = (page) => setCurrentPage(page)

  const updateAuthorRequestStatus = (id, newStatus) => {
    setAuthorRequests((prevRequests) => prevRequests.map((request) =>
      request.id == id ? { ...request, status: newStatus } : request
    ))
  }

  const handleAcceptRequest = async (idRequest) => {
    try {
      const token = localStorage.getItem("token")
      if (!token) {
        throw new Error("Token not found")
      }

      const response = await fetch(`api/AuthorRequests/accept?id=${idRequest}`, {
        method: "PUT",
        headers: {
          "Authorization": `Bearer ${token}`,
          "Content-Type": "application/json"
        },
        body: JSON.stringify({ Status: 2, IdRole: 2 })
      })

      const data = await response.json()
      if (!response.ok) {
        console.log(data.message)
      } else {
        updateAuthorRequestStatus(idRequest, 2)
      }
    } catch (error) {
      console.log(error)
    }
  }

  const handleRejectRequest = async (idRequest) => {
    try {
      const token = localStorage.getItem("token")
      if (!token) {
        throw new Error("Token not found")
      }

      const response = await fetch(`api/AuthorRequests/reject?id=${idRequest}`, {
        method: "PUT",
        headers: {
          "Authorization": `Bearer ${token}`,
          "Content-Type": "application/json"
        },
        body: JSON.stringify({ Status: 3, IdRole: 3 })
      })

      const data = await response.json()
      if (!response.ok) {
        console.log(data.message)
      } else {
        updateAuthorRequestStatus(idRequest, 3)
      }
    } catch (error) {
      console.log(error);
    }
  }

  const statusIcon = {
    1: { icon: <HiOutlineClock />, className: 'text-yellow-500', text: "Pending" },
    2: { icon: <HiOutlineCheck />, className: 'text-green-500', text: "Accepted" },
    3: { icon: <HiOutlineX />, className: 'text-red-500', text: "Rejected" },
  }

  // eslint-disable-next-line react/prop-types
  const AuthorRequestStatus = ({ status }) => {
    const { icon, className, text } = statusIcon[status] || {}
    return <div className={className}><p className="flex">{text} <span className="ml-3 text-xl">{icon}</span></p> </div>
  }

  return <div className="table-container-scrollbar table-auto overflow-x-scroll md:mx-auto p-3 scrollbar scrollbar-track-slate-100 scrollbar-thumb-slate-300 dark:scrollbar-track-slate-700 dark:scrollbar-thumb-slate-500">
    {currentUser.roleName === 'Admin' && authorRequests.length > 0 ? (
      <>
        <Table hoverable className="shadow-md my-8">
          <Table.Head>
            <Table.HeadCell>Date created</Table.HeadCell>
            <Table.HeadCell>User image</Table.HeadCell>
            <Table.HeadCell>Username</Table.HeadCell>
            <Table.HeadCell>Reason</Table.HeadCell>
            <Table.HeadCell>Status</Table.HeadCell>
            <Table.HeadCell>Change Status</Table.HeadCell>
          </Table.Head>

          {authorRequests.map((authorRequest) => (
            <Table.Body key={authorRequest.id} className="divide-y">
              <Table.Row className="bg-white dark:border-gray-700 dark:bg-gray-800">
                <Table.Cell>{new Date(authorRequest.dateCreated).toLocaleString()}</Table.Cell>
                <Table.Cell>
                  <Link to={`/user/${authorRequest.idUser}`}>
                    {authorRequest.profilePicture.startsWith('http') ? (<img src={authorRequest.profilePicture} className="w-10 h-10 object-cover bg-gray-500 rounded-full" />
                    ) : (<img src={`/api/Users/images/${authorRequest.profilePicture}`} className="w-10 h-10 object-cover bg-gray-500 rounded-full" />
                    )}
                  </Link>
                </Table.Cell>
                <Table.Cell>
                  <span>{authorRequest.username}</span>
                </Table.Cell>
                <Table.Cell className="w-64">
                  {authorRequest.reason}
                </Table.Cell>
                <Table.Cell className="w-64">
                  <AuthorRequestStatus status={authorRequest.status} />
                </Table.Cell>
                <Table.Cell className="flex">
                  <Button className="mr-3" color="failure" onClick={() => handleRejectRequest(authorRequest.id)}>Reject</Button>
                  <Button className="ml-6" color="success" onClick={() => handleAcceptRequest(authorRequest.id)}>Accept</Button>
                </Table.Cell>
              </Table.Row>
            </Table.Body>
          ))}
        </Table>

        <Pagination
          currentPage={currentPage}
          onPageChange={onPageChange}
          totalPages={pageCount}
          className="pb-6"
        />
      </>
    ) : (<p>No requests</p>)
    }
  </div>
}
