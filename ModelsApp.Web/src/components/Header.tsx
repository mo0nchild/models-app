import { Avatar } from '@mui/material';
import React from 'react';
import { Container, Nav, Navbar } from 'react-bootstrap';
import { useLocation } from 'react-router-dom';

export type UserData = {
    readonly name: string,
    readonly image: string | null
}
interface HeaderProps {
    readonly user: UserData | null
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
    const createLinkStyle = (path: string): React.CSSProperties => {
        const underlinkEnable = path == window.location.pathname;
        return {
            textDecoration: underlinkEnable ? 'underline' : 'none',
            textUnderlineOffset: '5px', ...navLinkStyle
        }
    }
    return (
    <div style={headerStyle}>
    <Navbar expand='md' style={{backgroundColor: '#242424'}} variant='dark'>
        <Container>
            <Navbar.Brand style={{color: '#FFF'}}>YourModels</Navbar.Brand>
            <Navbar.Toggle/>
            <Navbar.Collapse color='#FFF' style={{padding: '10px 0px'}}>
                <Nav className='me-auto'>
                    <Nav.Link href={'/'} style={createLinkStyle('/')}>Каталог</Nav.Link>
                    <Nav.Link href={'/create'} style={createLinkStyle('/create')}>Загрузить модель</Nav.Link>
                </Nav>
                <div style={{backgroundColor: '#FFF', height: '1px', margin: '5px 0px'}}></div>
                { user != null ?  userPanel() : loginPanel() }
            </Navbar.Collapse>
        </Container>
    </Navbar>
    </div>
    )
}
export type HeaderHandler = {
    readonly updateUser: () => Promise<void>
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
