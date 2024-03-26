import { createSlice } from '@reduxjs/toolkit';

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
      state.loading = true;
      state.error = null;
    },
    signInSuccess: (state, action) => {
      const payload = typeof action.payload === 'string' ? JSON.parse(action.payload) : action.payload;

      state.currentUser = {
        id: payload.Id,
        username: payload.Username,
        firstName: payload.FirstName,
        lastName: payload.LastName,
        email: payload.Email,
        profilePicture: payload.ProfilePicture
      };
      
      state.loading = false;
      state.error = null;
    },
    signInFailure: (state, action) => {
      state.loading = false;
      state.error = action.payload;
    }
  }
})

export const { signInStart, signInSuccess, signInFailure } = userSlice.actions;

export default userSlice.reducer;