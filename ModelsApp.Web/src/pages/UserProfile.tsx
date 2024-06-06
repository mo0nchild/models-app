import React from 'react';
import { useApiAccessor } from '@services/ApiAccess';
import UserInfo from '@components/UserInfo';
import { Button, Col, Container, Row } from 'react-bootstrap';

import UpdateModal, { UpdateInfo } from '@components/userprofile/UpdateModal';
import { Backdrop, CircularProgress } from '@mui/material';

import '@styles/DarkModal.css'
import '@styles/BootstrapFix.css'
import { Trash } from 'react-bootstrap-icons';
import { validationFormFactory } from '@services/ValidationField';
import { ErrorInfo, ErrorInfoHandler } from '@components/ErrorInfo';

export type UserProfileState = {
    name: string,
    email: string,
    biography: string,
    imageName: string
}
export default function UserProfile(): React.JSX.Element {
    const { accessor, logout } = useApiAccessor()
    const [ info, setInfo ] = React.useState<UserProfileState | null>(null);
    const [ updateShow, setUpdateShow ] = React.useState<boolean>(false);
    
    const errorRef = React.useRef<ErrorInfoHandler>(null);
    const [loaded, setLoaded] = React.useState<boolean>(false);
    const apiFetch = async () => {
        setLoaded(false);
        const result = await accessor<UserProfileState>({ 
            url: 'http://localhost:8080/modelsapp/profile/getInfo' 
        });
        setInfo({...result})
        setLoaded(true);
    }
    React.useEffect(() => {
        apiFetch().catch(error => console.log((error as Error).message));
    }, []);
    const renderUserInfo = (): React.JSX.Element => {
        const { biography, email, imageName, name } = info!;
        return (<UserInfo name={name} image={imageName} email={email} biography={biography}/>)
    }
    const deleteUserHandler = React.useCallback(async () => {
        await accessor({url: 'http://localhost:8080/modelsapp/profile/delete', method: 'DELETE'})
        logout();
    }, [])
    const updateUserHandler = React.useCallback<(info: UpdateInfo) => void>(async (info) => {
        const { name, biography, image } = info;
        const formData = new FormData();
        formData.append('name', name);
        formData.append('biography', biography);
        if(image != undefined) {
            formData.append('image', new File([image!], 'avatar.png'));
        }
        const validation = validator(formData);
        if (validation != null) {
            errorRef.current?.setError(validation);
            window.scrollTo(0, 0)
            return;
        }
        await accessor({ 
            url: 'http://localhost:8080/modelsapp/profile/update',
            method: 'PUT',
            body: formData
        }).catch(error => console.log((error as Error).message)) 
        setUpdateShow(false)
        await apiFetch().catch(error => console.log((error as Error).message));
    }, [])
    return (
    loaded ? <div>
        <Container fluid={'md'}>
            <Row className='justify-content-center'>
                <Col sm='12' md='10' lg='8' xl='6'>
                    <ErrorInfo ref={errorRef}/>
                </Col>
            </Row>
            <Row className='justify-content-center'>
                <Col sm='12' md='10' lg='10' xl='8'>
                    <div className='p-4 p-md-5 panel-info'>
                        <h1 className='fs-4 mb-4'>Информация о пользователе:</h1>
                        { info == null ? <div></div> : renderUserInfo() }
                        <div className='my-4' style={{display: 'flex', justifyContent: 'center'}}>
                            <div style={{ border: '0px', backgroundColor: '#FFF',  height: '1px',  width: '60%'}}></div>
                        </div>
                        <Container className='my-2'>
                            <Row className='justify-content-center gy-3'>
                                <Col xs={3} md={2} lg={2}>
                                    <Button className='button-active' onClick={deleteUserHandler}
                                        style={{backgroundColor: '#993d37', width: '100%' }}>
                                        <Trash width={22} height={22}/>
                                    </Button>
                                </Col>
                                <Col xs={9} md={5} lg={4}>
                                    <Button className='button-active' style={{width: '100%'}}
                                        onClick={() => {
                                            errorRef.current?.closeError();
                                            setUpdateShow(true)
                                        }}>
                                        Изменить
                                    </Button>
                                </Col>
                                <Col md={5} lg={4}>
                                    <Button className='button-active' style={{width: '100%'}} 
                                        onClick={() => logout()}>
                                        Выйти
                                    </Button>
                                </Col>
                                
                            </Row>
                        </Container>
                        <UpdateModal show={updateShow} onClose={() => setUpdateShow(false)}
                            onAccept={updateUserHandler} 
                            currentInfo={info == null ? undefined : {
                                name: info.name,
                                biography: info.biography,
                                image: info.imageName
                            }}/>
                    </div>
                </Col>
            </Row>
        </Container>
    </div> 
    : <Backdrop sx={{ color: '#fff', zIndex: (theme) => theme.zIndex.drawer + 1 }}
        open={!loaded}>
        <CircularProgress color="inherit" />
    </Backdrop>
    )
}
const validator = validationFormFactory([
    { 
        name: 'name',
        rule: (value) =>  value.length >= 4, 
        message: 'Длина имени больше 4 символов',  
    },
])