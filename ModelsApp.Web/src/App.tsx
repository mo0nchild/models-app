/* eslint-disable @typescript-eslint/ban-types */
import React from 'react'
import { Header, HeaderHandler } from '@components/Header';
import Footer from '@components/Footer';
import { RouterProvider, createBrowserRouter } from 'react-router-dom';

import '@core/App.css'
import 'bootstrap/dist/css/bootstrap.min.css';
import Authorization from '@pages/Authorization';
import CatalogList from '@pages/CatalogList';
import UserProfile from '@pages/UserProfile';
import Registration from '@pages/Registration';
import CreateModel from '@pages/CreateModel';
import ModelViewing from '@pages/ModelViewing';

export const headerRef = React.createRef<HeaderHandler>();

function App(): React.JSX.Element {
	const router = createBrowserRouter([
		{
			path: '/',
			element: <CatalogList />,
		},
		{
			path: '/model/:guid',
			element: <ModelViewing />
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
		},
		{
			path: '/create',
			element: <CreateModel />
		}
	]);
	React.useEffect(() => {
		headerRef.current?.updateUser()
	}, [])
	return (
		<div className={'page-content'}>
			<Header ref={headerRef}/>
			<div style={{flexGrow: '1', marginTop: '70px', color: '#FFF'}}
				className='py-1 py-md-5'>
				<RouterProvider router={router} />
			</div>
			<Footer />
		</div>
	);
}
export default App;

