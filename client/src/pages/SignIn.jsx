import { Alert, Button, Label, Spinner, TextInput } from 'flowbite-react'
import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom'
import { useDispatch } from 'react-redux';
import { jwtDecode } from 'jwt-decode';

import OAuth from '../components/OAuth';

import { signInStart, signInSuccess, signInFailure } from '../redux/user/userSlice';


export default function SignUp() {
  const [formData, setFormData] = useState({});
  const [errorMessages, setErrorMessage] = useState([]);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const dispatch = useDispatch();

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.id]: e.target.value.trim() })
  }

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      dispatch(signInStart());

      const response = await fetch('/api/Token', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(formData)
      });

      setLoading(false);

      if (!response.ok) {
        const data = await response.json();

        let validationErrors = [];

        if (Array.isArray(data.errors)) {
          validationErrors = data.errors.map(error => {
            error.fieldName === 'FirstName' ? 'First Name' : (error.fieldName === 'LastName' ? 'Last Name' : error.fieldName);
            return `${error.ErrorMessage}`;
          });
        } else if (typeof data.errors === 'object') {
          validationErrors = Object.entries(data.errors).map(([fieldName]) => {
            const fortmattedFieldName = fieldName === 'FirstName' ? 'First Name' : (fieldName === 'LastName' ? 'Last Name' : fieldName);
            return `${fortmattedFieldName} is required.`;
          }).flat();
        } else {
          dispatch(signInFailure(data.message));
        }

        setErrorMessage(validationErrors);
      } else {
        const { token } = await response.json();

        const decodedToken = jwtDecode(token);
        const userProfile = decodedToken.ActorData;

        localStorage.setItem('token', token);

        dispatch(signInSuccess(userProfile));

        navigate('/');
        setErrorMessage([]);
      }
    } catch (error) {
      dispatch(signInFailure(error));
    }
  }

  return (
    <div className='min-h-screen mt-20'>
      <div className='flex p-3 max-w-3xl mx-auto flex-col md:flex-row md:items-center gap-5'>
        <div className='flex-1'>
          <Link to='/' className='font-bold dark:text-white text-4xl'>
            <span className='px-2 py-1 bg-gradient-to-r from-indigo-500 via-purple-500 to-pink-500 rounded-lg text-white'>Marko&apos;s</span>
            Blog
          </Link>

          <p className='text-sm mt-5'>This is a demo project. You can sign in with your email and password or with Google.</p>
        </div>

        <div className='flex-1'>
          <form className='flex flex-col gap-4' onSubmit={handleSubmit}>
            <div>
              <Label value='Your username' />
              <TextInput type='text' placeholder='Username' id='username' onChange={handleChange} />
            </div>
            <div>
              <Label value='Your password' />
              <TextInput type='password' placeholder='********' id='password' onChange={handleChange} />
            </div>

            <Button gradientDuoTone='purpleToPink' type='submit' disabled={loading}>
              {
                loading ? <>(<Spinner size='sm' /> <span className='pl-3'>Loading...</span>)</> : 'Sign in'
              }
            </Button>

            <OAuth />
          </form>

          <div className='flex gap-2 text-sm mt-5'>
            <span>Dont have an account?</span>
            <Link to='/sign-up' className='text-blue-500'>
              Sign Up
            </Link>
          </div>

          {errorMessages && errorMessages.length > 0 && (
            <Alert className='mt-5' color='failure'>
              {errorMessages.map((error, index) => (
                <div key={index}>{error}</div>
              ))}
            </Alert>
          )}

        </div>
      </div>
    </div>
  )
}
