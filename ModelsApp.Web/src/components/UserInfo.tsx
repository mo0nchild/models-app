import React from 'react';
import { Col, Container, Form, Row } from 'react-bootstrap';

export interface UserInfoProps {
    name: string;
    email: string;
    biography: string;
    image: string;
}
export default function UserInfo(props: UserInfoProps): React.JSX.Element {
    const { name, email, biography, image } = props
    return (
    <Container>
        <Row className='justify-content-center mb-3'>
            <Col xs={8} md={6} lg={4} xl={4}>
                <img src={image == null ? '/profile-user.png' : image} alt='avatar' style={{
                    width: '100%',
                    objectFit: 'cover',
                    borderRadius: '120px',
                    border: '2px solid #FFF',
                    backgroundColor: '#323232'
                }}/>
            </Col>
        </Row>
        <Row className='justify-content-center gy-2 mb-3'>
            <Col md={6} lg={5} style={infoContentStyle}>
                <Form.Label>Имя пользователя:</Form.Label>
                <Form.Control style={infoFieldStyle} type='text' defaultValue={name} readOnly/>
            </Col>
            <Col md={6} lg={5} style={infoContentStyle}>
                <Form.Label >Email:</Form.Label>
                <Form.Control style={infoFieldStyle} type='text' defaultValue={email} readOnly/>
            </Col>
        </Row>
        <Row className='justify-content-center gy-2'>
            <Col md={12} lg={10} style={infoContentStyle}>
                <Form.Label>Биография:</Form.Label>
                <Form.Control style={infoFieldStyle} as="textarea" rows={3} readOnly
                    defaultValue={biography == '' || biography == null ? 'Не указано' : biography} />
            </Col>
        </Row>
    </Container>
    )
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
