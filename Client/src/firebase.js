// Import the functions you need from the SDKs you need
import { initializeApp } from "firebase/app"
// import { getAnalytics } from "firebase/analytics"

// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

// Your web app's Firebase configuration
// For Firebase JS SDK v7.20.0 and later, measurementId is optional

const firebaseConfig = {
  apiKey: import.meta.env.VITE_FIREBASE_API_KEY,
  authDomain: "blog-a6b98.firebaseapp.com",
  projectId: "blog-a6b98",
  storageBucket: "blog-a6b98.appspot.com",
  messagingSenderId: "311198757906",
  appId: "1:311198757906:web:3023a83a49eeb68fa494cb",
  measurementId: "G-TJWVQ5W4KH"
}

// Initialize Firebase
export const app = initializeApp(firebaseConfig)
