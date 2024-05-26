import { Avatar } from '@mui/material';
import React from 'react';
import { Container, Nav, Navbar } from 'react-bootstrap';

export type UserData = {
    name: string,
    image: string | null
}
interface HeaderProps {
    user: UserData | null
}
function HeaderComponent({user} : HeaderProps): React.JSX.Element {
    const userPanel = (): JSX.Element => {
        const { name, image } = user!;
        return (
        <a href={'/profile'} className='mt-md-0 mt-3' style={profileLinkStyle}>
            <span style={{marginRight: '10px'}}>{name}</span>
            <Avatar alt='...' src={image ?? undefined} />
        </a>
        );
    }
    const loginPanel = (): JSX.Element => {
        return (
        <a href={'/login'} className='mt-md-0 mt-3' style={profileLinkStyle}>
            <span style={{marginRight: '10px'}}>Войти в профиль</span>
        </a>
        )
    }
    return (
    <div style={headerStyle}>
    <Navbar expand='md' style={{backgroundColor: '#242424'}} variant='dark'>
        <Container>
            <Navbar.Brand style={{color: '#FFF'}}>YourModels</Navbar.Brand>
            <Navbar.Toggle/>
            <Navbar.Collapse color='#FFF' style={{padding: '10px 0px'}}>
                <Nav className='me-auto'>
                    <Nav.Link href={'/'} style={navLinkStyle}>Главная</Nav.Link>
                    <Nav.Link href={'/'} style={navLinkStyle}>Каталог</Nav.Link>
                </Nav>
                <div style={{backgroundColor: '#FFF', height: '1px'}}></div>
                { user != null ?  userPanel() : loginPanel() }
            </Navbar.Collapse>
        </Container>
    </Navbar>
    </div>
    )
}
export type HeaderHandler = {
    updateUser: () => Promise<void>
}
export const Header = React.forwardRef<HeaderHandler, {}>((_, ref) => {
    const [ userData, setUserData ] = React.useState<UserData | null>(null);
    React.useEffect(() => { }, []);
    React.useImperativeHandle(ref, () => ({
        updateUser: async () => {
            const jwtToken = window.localStorage.getItem('jwtToken')
            if(jwtToken == null) {
                setUserData(null);
                return;
            }
            const response = await window.fetch('http://localhost:8080/modelsapp/profile/getInfo', {
                method: 'GET',
                headers: {
                    'ApiKey': 'd98bfe8d-0127-4708-b537-08708f72698b',
                    'Authorization': `Bearer ${jwtToken}`
                }
            })
            if(response.status != 200) {
                setUserData(null)
                return;
            }
            const userData = await response.json();
            setUserData({
                name: userData.name,
                image: userData.imageName
            });
        },
    }));
    return (<HeaderComponent user={userData} />)
});

const headerStyle: React.CSSProperties = {
    position: 'fixed',
    width: '100%',
    zIndex: '1',
    boxShadow: '0px 0px 20px #888',
}
const navLinkStyle: React.CSSProperties = {
    color: '#FFF'
}
const profileLinkStyle: React.CSSProperties = {
    display: 'flex',
    flexFlow: 'row',
    alignItems: 'center',
    color: '#FFF',
    justifyContent: 'end',
    textDecoration: 'none'
}
