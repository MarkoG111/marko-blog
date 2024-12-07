import { Pagination } from "flowbite-react";
import { useState, useEffect } from "react"
import { Link } from "react-router-dom";
import { useError } from "../contexts/ErrorContext";

export default function Authors() {
  const [authors, setAuthors] = useState([]);
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
          console.log(response);

          const data = await response.json()
          console.log(data);

          const authors = data.items.filter((author) => author.role == 'Author')

          setAuthors(authors)
          setPageCount(data.pageCount)
        } else {
          const errorText = await response.text()
          const errorData = JSON.parse(errorText)

          if (Array.isArray(errorData.errors)) {
            errorData.errors.forEach((err) => {
              showError(err.ErrorMessage)
            })
          } else {
            const errorMessage = errorData.message || "An unknown error occurred.";
            showError(errorMessage)
          }

          return
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
                <img src={author.profilePicture} alt="profilePicture" className="w-32 object-cover rounded-full" />
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
