import React from 'react';
import { Button, Col, Container, Form, Row } from 'react-bootstrap';
import { getUserInfo, useApiAccessor, useCheckUser } from '@services/ApiAccess';
import { useNavigate } from 'react-router-dom';
import { ErrorInfo, ErrorInfoHandler } from '@components/ErrorInfo';
import { validationFormFactory } from '@services/ValidationField';

export default function Registration(): React.JSX.Element {
    const errorRef = React.useRef<ErrorInfoHandler>(null);

    const loginRef = React.useRef<HTMLInputElement>(null);
    const passwordRef = React.useRef<HTMLInputElement>(null);

    const nameRef = React.useRef<HTMLInputElement>(null);
    const emailRef = React.useRef<HTMLInputElement>(null);

    const { accessor, authorization } = useApiAccessor();
    const navigator = useNavigate();
    const checkAuth = useCheckUser()
    React.useEffect(() => {
        if (getUserInfo().jwtToken != null) navigator('/profile');
    }, [])
    const registrationHandler = React.useCallback(async () => {
        const formData = new FormData();

        formData.append('login', loginRef.current!.value);
        formData.append('password', passwordRef.current!.value);
        formData.append('name', nameRef.current!.value);
        formData.append('email', emailRef.current!.value);

        const validation = registrationValidator(formData);
        if (validation != null) {
            errorRef.current?.setError(validation);
            window.scrollTo(0, 0)
            return;
        }
        await accessor({
            url: 'http://localhost:8080/modelsapp/auth/registration', 
            method: 'POST',
            body: formData
        })
            .catch(error => console.log((error as Error).message))

        await authorization({
            login: loginRef.current!.value,
            password: passwordRef.current!.value
        })
            .catch(error => console.log((error as Error).message))
        navigator('/profile')
    }, []);
    const renderRegistrationForm = (): React.JSX.Element => {
        return (
        <div>
            <Form.Group className="mb-3 px-3" style={infoContentStyle}>
                <Form.Label>Логин:</Form.Label>
                <Form.Control className='form-control-fix' ref={loginRef} placeholder="Ваше имя"
                    style={infoFieldStyle} maxLength={20} />
            </Form.Group>
            <Form.Group className="mb-4 px-3" style={infoContentStyle}>
                <Form.Label>Пароль:</Form.Label>
                <Form.Control className='form-control-fix'  ref={passwordRef} type='password' 
                    placeholder='Пароль доступа' maxLength={50} style={infoFieldStyle}/>
            </Form.Group>
            <div className='my-4 mt-5' style={{display: 'flex', justifyContent: 'center'}}>
                <div style={{ border: '0px', backgroundColor: '#FFF',  height: '1px',  width: '100%'}}></div>
            </div>
            <Form.Group className="mb-3 px-3" style={infoContentStyle}>
                <Form.Label>Имя пользователя:</Form.Label>
                <Form.Control className='form-control-fix' ref={nameRef} type="email" placeholder="Ваше имя"
                    style={infoFieldStyle} maxLength={20} />
            </Form.Group>
            <Form.Group className="mb-4 px-3" style={infoContentStyle}>
                <Form.Label>Email:</Form.Label>
                <Form.Control className='form-control-fix' ref={emailRef} placeholder='Электронная почта'
                    style={infoFieldStyle} maxLength={50}/>
            </Form.Group>
            <Button className='button-active mt-3' style={{width: '80%'}} onClick={registrationHandler}>
                Создать профиль
            </Button>
        </div>
        );
    }
    return (
    <div >
    <Container fluid={'md'}>
        <Row className='justify-content-center'>
            <Col sm='12' md='10' lg='8' xl='6'>
                <ErrorInfo ref={errorRef}/>
            </Col>
        </Row>
        <Row className='justify-content-center'>
            
            <Col sm='12' md='10' lg='8' xl='6'>
                <div className='p-4 p-md-5 panel-info'>
                    <h1 className='fs-4 mb-4'>Регистрация:</h1>
                    { renderRegistrationForm() }
                </div>
            </Col>
        </Row>
    </Container>
    </div>
    )
}
const registrationValidator = validationFormFactory([
    { 
        name: 'login',
        rule: (value) =>  value.length >= 5, 
        message: 'Длина логина больше 5 символов',  
    },
    { 
        name: 'password',
        rule: (value) =>  value.length >= 5, 
        message: 'Длина пароля больше 5 символов',  
    },
    { 
        name: 'name',
        rule: (value) =>  value.length >= 4, 
        message: 'Длина имени больше 4 символов',  
    },
    { 
        name: 'email',
        rule: (value) => value.length >= 5 && /^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$/.test(value), 
        message: 'Формат email не соответсвует',  
    },
]);
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