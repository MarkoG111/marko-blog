import PropTypes from 'prop-types'
import { useEffect, useState } from "react"
import { useSelector } from "react-redux"
import { Link } from "react-router-dom"
import { HiOutlineClock, HiOutlineCheck, HiOutlineX } from 'react-icons/hi'
import { Table, Pagination, Button } from "flowbite-react"
import { useError } from "../contexts/ErrorContext"
import { handleApiError } from "../utils/handleApiUtils"
import { getAvatarSrc } from '../utils/getAvatarSrc'

export default function DashAuthorRequests() {
  const { currentUser } = useSelector((state) => state.user)

  const [authorRequests, setAuthorRequests] = useState([])
  const [currentPage, setCurrentPage] = useState(1)
  const [pageCount, setPageCount] = useState(1)
  const [imageError, setImageError] = useState(false)

  const { showError } = useError()

  useEffect(() => {
    const fetchAuthorRequests = async () => {
      try {
        const token = localStorage.getItem("token")
        if (!token) {
          throw new Error("Token not found")
        }

        const response = await fetch(`/authorrequests?page=${currentPage}`, {
          method: "GET",
          headers: {
            "Authorization": `Bearer ${token}`
          }
        })

        if (response.ok) {
          const data = await response.json()

          setAuthorRequests(data.items)
          setPageCount(data.pageCount)
        } else {
          await handleApiError(response, showError)
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

  const handleAcceptAuthorRequest = async (idRequest) => {
    try {
      const token = localStorage.getItem("token")
      if (!token) {
        throw new Error("Token not found")
      }

      const response = await fetch(`/authorrequests/accept?id=${idRequest}`, {
        method: "PUT",
        headers: {
          "Authorization": `Bearer ${token}`,
          "Content-Type": "application/json"
        },
        body: JSON.stringify({ status: 2, idRole: 2 })
      })

      if (response.ok) {
        updateAuthorRequestStatus(idRequest, 2)
      } else {
        await handleApiError(response, showError)
      }
    } catch (error) {
      showError(error.message)
    }
  }

  const handleRejectAuthorRequest = async (idRequest) => {
    try {
      const token = localStorage.getItem("token")
      if (!token) {
        throw new Error("Token not found")
      }

      const response = await fetch(`/authorrequests/reject?id=${idRequest}`, {
        method: "PUT",
        headers: {
          "Authorization": `Bearer ${token}`,
          "Content-Type": "application/json"
        },
        body: JSON.stringify({ status: 3, idRole: 3 })
      })

      if (response.ok) {
        updateAuthorRequestStatus(idRequest, 3)
      } else {
        await handleApiError(response, showError)
      }
    } catch (error) {
      showError(error.message)
    }
  }

  const statusIcon = {
    1: { icon: <HiOutlineClock />, className: 'text-yellow-500', text: "Pending" },
    2: { icon: <HiOutlineCheck />, className: 'text-green-500', text: "Accepted" },
    3: { icon: <HiOutlineX />, className: 'text-red-500', text: "Rejected" },
  }

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
                    <img
                      src={getAvatarSrc(authorRequest.profilePicture)}
                      className="w-10 h-10 object-cover bg-gray-500 rounded-full"
                      onError={() => setImageError(true)}
                    />
                  </Link>
                </Table.Cell>
                <Table.Cell>
                  <span>{authorRequest.username}</span>
                </Table.Cell>
                <Table.Cell className="w-64">
                  {authorRequest.reason}
                </Table.Cell>
                <Table.Cell className="w-64">
                  <AuthorRequestStatus status={authorRequest.status || 1} />
                </Table.Cell>
                <Table.Cell className="flex">
                  <Button className="mr-3" color="failure" onClick={() => handleRejectAuthorRequest(authorRequest.id)}>Reject</Button>
                  <Button className="ml-6" color="success" onClick={() => handleAcceptAuthorRequest(authorRequest.id)}>Accept</Button>
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

DashAuthorRequests.propTypes = {
  status: PropTypes.number
}