import { useSelector } from "react-redux"
import { Navigate, Outlet } from "react-router-dom"

export default function OnlyRolePrivateRoute() {
  const { currentUser } = useSelector((state) => state.user)

  return currentUser && (currentUser.roleName == 'Admin' || currentUser.roleName == 'Author') ? <Outlet /> : <Navigate to='/sign-in' />
}
