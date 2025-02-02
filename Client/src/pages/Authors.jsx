import { useState, useEffect } from "react"
import { Link } from "react-router-dom"
import { Pagination } from "flowbite-react"
import { useError } from "../contexts/ErrorContext"
import { handleApiError } from "../utils/handleApiUtils"
import { getAvatarSrc } from "../utils/getAvatarSrc"

export default function Authors() {
  const [authors, setAuthors] = useState([])
  const [currentPage, setCurrentPage] = useState(1)
  const [pageCount, setPageCount] = useState(1)

  const { showError } = useError()

  useEffect(() => {
    const fethcAuthors = async () => {
      try {
        const queryParams = new URLSearchParams({
          onlyAuthors: true,
          perPage: 3,
          page: currentPage
        })

        const response = await fetch(`/users?${queryParams}`, {
          method: "GET"
        })

        if (response.ok) {
          const data = await response.json()

          const authors = data.items.filter((author) => author.role == 'Author')

          setAuthors(authors)
          setPageCount(data.pageCount)
        } else {
          await handleApiError(response, showError)
        }
      } catch (error) {
        showError(error.message || "An unknown error occurred.")
      }
    }

    fethcAuthors()
  }, [currentPage, showError])

  const onPageChange = (page) => setCurrentPage(page)

  return (
    <div className="pb-9 mx-24 min-h-screen">
      <h1 className="text-center text-3xl my-7 font-semibold">Authors</h1>
      <div className="flex flex-wrap justify-center gap-12">{authors.length > 0 ? (
        authors.map(author => (
          <div key={author.id} className="flex flex-col flex-2 min-w-[250px] items-center dark:bg-gray-800 border border-teal-500 py-6 rounded-lg text-center">
            <Link to={`/user/${author.id}`}>
              <div className="flex flex-col items-center">
                <img src={getAvatarSrc(author.profilePicture)} alt="profilePicture" referrerPolicy="no-referrer" className="w-32 object-cover rounded-full" />
                <div className="py-8">
                  <p className="text-indigo-400 text-xl font-semibold">{author.firstName} {author.lastName}</p>
                  <p className="py-3">@{author.username}</p>
                  <p>{author.email}</p>
                </div>
              </div>
            </Link>
          </div>
        ))
      ) : (
        <p className="text-xl text-gray-500">No authors found</p>
      )}</div>

      <div className="flex justify-center my-12">
        {authors.length > 0 &&
          <Pagination
            currentPage={currentPage}
            onPageChange={onPageChange}
            totalPages={pageCount}
            className="text-l"
          />
        }
      </div>
    </div>
  )
}
