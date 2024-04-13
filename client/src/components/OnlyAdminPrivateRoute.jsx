import { useSelector } from "react-redux"
import { Navigate, Outlet } from "react-router-dom";

export default function OnlyAdminPrivateRoute() {
  const { currentUser } = useSelector((state) => state.user);

  return currentUser && currentUser.roleName == 'Admin' ? <Outlet /> : <Navigate to='/sign-in' />;
}
