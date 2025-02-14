import { Button } from "flowbite-react"

export default function CallToAction() {
  return (
    <div className="flex flex-col sm:flex-row p-3 mx-4 md:mx-0 border border-teal-500 justify-center items-center rounded-tl-3xl rounded-br-3xl text-center">
      <div className="flex-1 justify-center flex flex-col">
        <h2 className="text-2xl">Want to learn more about JavaScript?</h2>
        <p className="text-gray-500 my-2">Checkout these resources with 100 JavaScript Projects</p>
        <Button gradientDuoTone="purpleToPink"><a href="https://wwww.100jsprojects.com" target="_blank" rel="noopener noreferrer">100 JavaScript Projects</a></Button>
      </div>
      <div className="flex-1 p-7">
        <img src="https://images.ctfassets.net/8cjpn0bwx327/1kYEy9rLk5z7iD4m2WXW4b/e3f39ee5a02c0a6aac76f91f9d630e3a/JavaScript_in_Web_Development.jpg" alt="" />
      </div>
    </div>
  )
}
