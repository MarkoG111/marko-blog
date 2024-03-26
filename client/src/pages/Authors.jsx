import { useState, useEffect } from "react"

export default function Authors() {
  const [users, setUsers] = useState([]);

  useEffect(() => {
    fetch('/api/Users')
      .then(response => response.json())
      .then(data => setUsers(data.items))
      .catch(error => console.error('Error fetching users: ', error));
  }, []);

  return (
    <div>
      <h1>Authors</h1>
      {console.log(users)}
      <div>{users.length > 0 ? (
        users.map(user => (
          <div key={user.id}>
            <p>{user.firstName} {user.lastName}</p>
            <p>{user.email}</p>
          </div>
        ))
      ) : (
        <p>Loading...</p>
      )}</div>
    </div>
  )
}
