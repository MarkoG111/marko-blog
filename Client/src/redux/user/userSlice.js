import { createSlice } from '@reduxjs/toolkit'

const initialState = {
  currentUser: null,
  error: null,
  loading: false
}

const userSlice = createSlice({
  name: 'user',
  initialState,
  reducers: {
    signInStart: (state) => {
      state.loading = true
      state.error = null
    },
    signInSuccess: (state, action) => {
      const payload = typeof action.payload === 'string' ? JSON.parse(action.payload) : action.payload

      state.currentUser = {
        id: payload.Id,
        firstName: payload.FirstName,
        lastName: payload.LastName,
        username: payload.Identity,
        email: payload.Email,
        allowedUseCases: payload.AllowedUseCases,
        profilePicture: payload.ProfilePicture,
        roleName: payload.RoleName
      }

      state.loading = false
      state.error = null
    },
    signInFailure: (state, action) => {
      state.loading = false
      state.error = Array.isArray(action.payload) && action.payload.length ? action.payload : typeof action.payload === 'string' ? action.payload : "An unknown error occurred"
    },

    updateUserSuccess: (state, action) => {
      state.currentUser = action.payload
    },

    updateProfilePictureSuccess: (state, action) => {
      state.currentUser.profilePicture = action.payload
    },

    deleteUserSuccess: (state) => {
      state.currentUser = null
      state.loading = false
      state.error = null
    },
    deleteUserFailure: (state, action) => {
      state.loading = false
      state.error = action.payload
    },

    signoutSuccess: (state) => {
      state.currentUser = null
      state.loading = false
      state.error = null
    }
  }
})

export const { signInStart, signInSuccess, signInFailure, updateProfilePictureSuccess, deleteUserSuccess, deleteUserFailure, updateUserSuccess, signoutSuccess } = userSlice.actions

export default userSlice.reducer