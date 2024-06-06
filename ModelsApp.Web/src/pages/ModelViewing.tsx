import { ModelScene, ModelSceneHandler } from '@components/ModelScene';
import CommentInfo from '@components/modelviewing/CommentInfo';
import { Rating } from '@mui/material';
import StarIcon from '@mui/icons-material/Star'
import { getUserInfo, useApiAccessor, useCheckUser } from '@services/ApiAccess';
import React from 'react';
import { Badge, Button, Col, Container, Form, Row } from 'react-bootstrap';
import { useParams } from 'react-router-dom';
import * as THREE from 'three';
import { Eye } from 'react-bootstrap-icons';
import UserCard from '@components/UserCard';

export interface ModelInfoApi {
    guid: string;
    name: string;
    description: string;
    views: string;
    rating: number;
    categoryName: string;
    imageName: string;
    owner: {
        guid: string;
        name: string;
        email: string;
        biography: string;
        imageName: string;
    }
    info: {
        vertices: number;
        triangles: number;
        memorySize: number;
        lightIntensity: number;
        sceneColor: string;
        cameraX: number;
        cameraY: number;
        cameraZ: number;
        targetX: number;
        targetY: number;
        targetZ: number;
    }
}
export interface CommentListApi {
    items: {
        rating: number,
        text: string,
        user: {
            name: string,
            imageName: string
        }
    }[];
    allCount: number
}
export default function ModelViewing(): React.JSX.Element {
    const sceneRef = React.useRef<ModelSceneHandler | null>(null);
    const { guid } = useParams();
    const { accessor } = useApiAccessor();
    const { checkUser } = useCheckUser();
    
    const [ modelData, setModelData ] = React.useState<Blob | null>(null);
    const [ modelInfo, setModelInfo ] = React.useState<ModelInfoApi | null>(null);
    
    const [ authenticated, setAuthenticated ] = React.useState<boolean>();
    const [ comments, setComments ] = React.useState<CommentListApi | null>(null);
    const [ commentText, setCommentText ] = React.useState<string>('');
    const [ rating, setRating ] = React.useState<number>(0);
    React.useEffect(() => {
        const getModelInfo = async () => {
            const infoUrl = `http://localhost:8080/modelsapp/models/getInfo?uuid=${guid}`;
            setModelInfo(await accessor<ModelInfoApi>({url: infoUrl}))
            
            const commentsUrl = `http://localhost:8080/modelsapp/comments/getList?modelUuid=${guid}`
            setComments(await accessor<CommentListApi>({url: commentsUrl}))

            const modelUrl = `http://localhost:8080/modelsapp/models/getData?uuid=${guid}`;
            const model = await window.fetch(modelUrl, {
                method: 'GET',
                headers: { 'ApiKey': 'd98bfe8d-0127-4708-b537-08708f72698b' }
            });
            setModelData(await model.blob());
            setAuthenticated(getUserInfo().uuid != null);
        }
        getModelInfo().catch(error => console.log(error));
    }, [])
    const downloadFileHandler = React.useCallback(async() => {
        const modelUrl = `http://localhost:8080/modelsapp/models/getData?uuid=${guid}`;
        const model = await window.fetch(modelUrl, {
            method: 'GET',
            headers: { 'ApiKey': 'd98bfe8d-0127-4708-b537-08708f72698b' }
        });
        const link = document.createElement('a');
        link.href = URL.createObjectURL(await model.blob());
        link.download = 'model.glb';
        link.click();
    }, [])
    const ownerCardHandler = () => { }
    const sendCommentHandler = React.useCallback(async () => {
        const formData = new FormData();
        formData.append('rating', rating.toString())
        formData.append('text', commentText);
        formData.append('modelUuid', guid!);
        await accessor({
            url: 'http://localhost:8080/modelsapp/comments/add', 
            method: 'POST',
            body: formData
        });

        const infoUrl = `http://localhost:8080/modelsapp/models/getInfo?uuid=${guid}`;
        setModelInfo(await accessor<ModelInfoApi>({url: infoUrl}))

        const commentsUrl = `http://localhost:8080/modelsapp/comments/getList?modelUuid=${guid}`
        setComments(await accessor<CommentListApi>({url: commentsUrl}))

    }, [commentText, rating])
    const renderViewPanel = (): React.JSX.Element => {
        if (modelInfo == null) return <div></div>
        const { cameraX, cameraY, cameraZ, targetX, targetY, targetZ } = modelInfo.info
        const { name, description, categoryName, rating, owner } = modelInfo
        const modelScene = modelData == null ? <div></div> :
            <ModelScene modelData={modelData} currentAnimation={null} ref={sceneRef} 
                sceneColor={modelInfo?.info.sceneColor ?? '#FFF'} 
                onSceneLoaded={(info) => {
                    info.animations
                }}
                cameraSettings={{
                    position: new THREE.Vector3(cameraX, cameraY, cameraZ),
                    target: new THREE.Vector3(targetX, targetY, targetZ)
                }}
                lightIntensity={modelInfo?.info.lightIntensity ?? 1} />
        return (
        <Container fluid={'md'}>
            <Row className='justify-content-start mb-4'>
                <Col md={12} style={fieldInfoStyle}>
                    <Form.Label>Превью модели:</Form.Label>
                    {modelScene}
                </Col>
            </Row>
            <Row className='justify-content-start mb-4 gy-2'>
                <Col xs={12} md={6} style={fieldInfoStyle}>
                    <Form.Label>Название:</Form.Label>
                    <Form.Control style={infoFieldStyle} type='text' readOnly defaultValue={name} />
                </Col>
                <Col xs={12} md={6} style={fieldInfoStyle}>
                    <Form.Label>Категория:</Form.Label>
                    <Form.Control style={infoFieldStyle} type='text' readOnly defaultValue={categoryName} />
                </Col>
            </Row>
            <Row className='justify-content-between mb-3 gy-2'>
                <Col xs={12} md={4} style={fieldInfoStyle}>
                    <UserCard onClick={ownerCardHandler} imageUrl={owner.imageName} name={owner.name}/>
                </Col>
                <Col xs={12} md={4}>
                    <div style={{
                        display: 'flex', alignItems: 'center', width: '100%', height: '100%'
                    }}>
                        <Rating value={rating} color={'#FFFFFF'} disabled
                            emptyIcon={<StarIcon style={{ opacity: 0.55, color: '#AAA' }} fontSize="inherit" />}/>
                        <div style={{ 
                            display: 'flex',
                            alignItems: 'center', 
                            marginLeft: '20px',
                            padding: '6px 10px',
                        }}>
                            <span style={{marginRight: '10px'}}>{modelInfo.views}</span>
                            <Eye color='#AAA' width={24} height={24}/>
                        </div>
                    </div>
                </Col>
            </Row>
            <div className='my-4' style={{display: 'flex', justifyContent: 'center'}}>
                <div style={{ border: '0px', backgroundColor: '#FFF',  height: '1px',  width: '100%'}}></div>
            </div>
            <Row className='justify-content-start mb-4 gy-2'>
                <Col xs={12} md={7} style={fieldInfoStyle}>
                    <Form.Label>Описание модели:</Form.Label>
                    <Form.Control style={infoFieldStyle} type='text' readOnly rows={3}
                        defaultValue={description} as='textarea' />
                </Col>
                <Col xs={12} md={5} style={fieldInfoStyle}>
                    <Form.Label>Информация модели:</Form.Label>
                    <div style={{textAlign: 'start'}}>
                        <span>Кол-во вершин: {modelInfo.info.vertices}</span>
                        <br/>
                        <span>Кол-во полигонов: {modelInfo.info.triangles}</span>
                        <br/>
                        <span>Размер файла: {modelInfo.info.memorySize}mb</span>
                    </div>
                </Col>
            </Row>
            <Row className='justify-content-center mb-2 gy-2'>
                <Col xs={12} md={4}>
                    <Button style={{width: '100%'}} onClick={downloadFileHandler}>Скачать</Button>
                </Col>
            </Row>
        </Container>
        )
    }
    const renderCommentPanel = (): React.JSX.Element => {
        return (
        <Container fluid={'md'}>
            {
            authenticated ? 
            <Row className=' justify-content-center'>
                <Col sm={8} style={fieldInfoStyle}>
                    <Form.Label>Текст комментария</Form.Label>
                    <Form.Control style={{margin: '0px 0px 20px', ...infoFieldStyle}} type='text' rows={3}
                        as='textarea' onChange={(item) => setCommentText(item.target.value)}
                        placeholder='Введите текст комментария'/>
                    <div style={{display: 'flex', alignItems: 'center'}}>
                        <Button style={{marginRight: '20px'}} onClick={sendCommentHandler}>Отправить</Button>
                        <Rating value={rating} color={'#FFFFFF'} onChange={(_, value) => setRating(value)}
                            emptyIcon={<StarIcon style={{ opacity: 0.55, color: '#AAA' }} fontSize="inherit" />}/>
                    </div>
                </Col>
            </Row>
            : <div>Необходимо авторизоваться, чтобы оставлять комментарии</div>
            }
            <div className='my-4' style={{display: 'flex', justifyContent: 'center'}}>
                <div style={{ border: '0px', backgroundColor: '#FFF',  height: '1px',  width: '100%'}}></div>
            </div>
            {
            comments == null ? <div></div> :
            <Row className='' style={{maxHeight: '800px', overflowY: 'scroll'}}>
                <Col sm={12}>
                    {
                    comments!.items.map(({rating, text, user}) => {
                        return (
                        <div style={{
                            border: '1px solid #FFF',
                            borderRadius: '10px',
                            padding: '20px'
                        }}>
                            <CommentInfo rating={rating} text={text} 
                                user={{image: user.imageName, name: user.name}}/>
                        </div>
                        )
                    })
                    }
                </Col>
            </Row>
            }
        </Container>
        )
    }
    return (
    <div style={{width: '100%'}}>
        <Container fluid={'md'}>
            <Row className='justify-content-center mb-5'>
                <Col sm='12' md='10' lg='10' xl='8'>
                    <div className='p-4 p-md-5 panel-info'>
                        <h1 className='fs-4 mb-4'>Просмотр модели:</h1>
                        {renderViewPanel()}
                    </div>        
                </Col>
            </Row>
            <Row className='justify-content-center mb-3'>
                <Col sm='12' md='10' lg='10' xl='8'>
                    <div className='p-4 p-md-5 panel-info'>
                        <h1 className='fs-4 mb-4'>Комментарии:</h1>
                        {renderCommentPanel()}
                    </div>        
                </Col>
            </Row>
        </Container>
    </div>
    )
}
const fieldInfoStyle: React.CSSProperties = {
    display: 'flex', 
    flexFlow: 'column', 
    alignItems: 'start'
}
const infoFieldStyle: React.CSSProperties = {
    backgroundColor: '#323232',
    color: '#FFF',
    resize: 'none'
}