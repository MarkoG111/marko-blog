import { Pagination } from "flowbite-react";
import { useState, useEffect } from "react"
import { Link } from "react-router-dom";

export default function Authors() {
  const [authors, setAuthors] = useState([]);
  const [currentPage, setCurrentPage] = useState(1)
  const [pageCount, setPageCount] = useState(1)

  useEffect(() => {
    const fethcAuthors = async () => {
      try {
        const queryParams = new URLSearchParams({
          page: currentPage,
          perPage: 3,
          onlyAuthors: true
        })

        const response = await fetch(`/api/Users?${queryParams}`, {
          method: "GET"
        })

        if (response.ok) {
          const data = await response.json()
          const authors = data.items.filter((author) => author.role == 'Author')
          
          setAuthors(authors)
          setPageCount(data.pageCount)
        }
      } catch (error) {
        console.log(error)
      }
    }

    fethcAuthors()
  }, [currentPage])

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
