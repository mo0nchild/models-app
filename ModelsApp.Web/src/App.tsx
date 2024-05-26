/* eslint-disable @typescript-eslint/ban-types */
import React from 'react'
import { Header, HeaderHandler } from './components/Header';
import Footer from './components/Footer';
import { RouterProvider, createBrowserRouter } from 'react-router-dom';

import './App.css'
import 'bootstrap/dist/css/bootstrap.min.css';
import Authorization from './pages/Authorization';
import Home from './pages/Home';
import UserProfile from './pages/UserProfile';
import Registration from './pages/Registration';

export const headerRef = React.createRef<HeaderHandler>();

function App(): React.JSX.Element {
	const router = createBrowserRouter([
		{
			path: '/',
			element: <Home />,
		},
		{
			path: '/login',
			element: <Authorization />
		},
		{
			path: '/registration',
			element: <Registration />
		},
		{
			path: '/profile',
			element: <UserProfile />
		}
	]);
	React.useEffect(() => {
		headerRef.current?.updateUser()
	}, [])
	return (
		<div className={'page-content'}>
			<Header ref={headerRef}/>
			<div style={{flexGrow: '1', marginTop: '70px', color: '#FFF'}}
				className='py-5 py-xs-2'>
				<RouterProvider router={router} />
			</div>
			<Footer />
		</div>
	);
}
export default App;

