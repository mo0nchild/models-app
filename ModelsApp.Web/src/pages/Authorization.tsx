import React from 'react';
import { headerRef } from '@core/App';
import { useNavigate } from 'react-router-dom';
import { useApiAccessor } from '@services/ApiAccess';
import { Button, Col, Container, Form, Row } from 'react-bootstrap';

import '@core/BootstrapFix.css'

export default function Authorization(): React.JSX.Element {
    const loginRef = React.useRef<HTMLInputElement>(null);
    const passwordRef = React.useRef<HTMLInputElement>(null);
    
    const navigator = useNavigate();
    const { authorization } = useApiAccessor();
    const loginHandler = React.useCallback(async () => {
        const login = loginRef.current!.value;
        const password = passwordRef.current!.value;

        authorization({ login: login, password: password })
            .then(() => {
                headerRef.current?.updateUser()
                navigator('/profile')
            })
            .catch(error => console.log((error as Error).message))
    }, []);
    const renderLoginForm = (): React.JSX.Element => {
        return (
        <div>
            <Form.Group className="mb-3 px-3" style={infoContentStyle}>
                <Form.Label>Логин:</Form.Label>
                <Form.Control className='form-control-fix' ref={loginRef} placeholder="Ваше имя"
                    style={infoFieldStyle} maxLength={50}/>
            </Form.Group>
            <Form.Group className="mb-4 px-3" style={infoContentStyle}>
                <Form.Label>Пароль:</Form.Label>
                <Form.Control className='form-control-fix'  ref={passwordRef} type='password' 
                    placeholder='Пароль доступа' style={infoFieldStyle} maxLength={50}/>
            </Form.Group>
            <p>У вас нет профиля? <a style={{color: '#FFF'}} href='/registration'>Создать</a></p>
            <Button className='button-active mt-3' style={{width: '80%'}} onClick={loginHandler}>
                Войти в профиль
            </Button>
        </div>
        )
    }
    return (
    <div className='mt-5'>
    <Container fluid={'md'}>
        <Row className='justify-content-center'>
            <Col sm='12' md='10' lg='8' xl='6'>
                <div className='p-4 p-md-5 panel-info'>
                    <h1 className='fs-4 mb-4'>Авторизация:</h1>
                    { renderLoginForm() }
                </div>
            </Col>
        </Row>
    </Container>
    </div>
    );
}
const infoFieldStyle: React.CSSProperties = {
    backgroundColor: '#323232',
    color: '#FFF',
    resize: 'none'
}
const infoContentStyle: React.CSSProperties = {
    display: 'flex', 
    flexFlow: 'column', 
    alignItems: 'start'
}