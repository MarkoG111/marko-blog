import { useSelector } from "react-redux"
import PropTypes from "prop-types"
export default function ThemeProvider({ children }) {
  const { theme } = useSelector(state => state.theme)

  return (
    <div className={theme}>
      <div className="bg-gray-50 text-gray-700 dark:text-gray-200 dark:bg-[rgb(16,32,42)] min-h-screen">
        {children}
      </div>
    </div>
  )
}

ThemeProvider.propTypes = {
  children: PropTypes.node.isRequired,
}