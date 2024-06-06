import React from "react";
import { Container, Form, Row, Col } from "react-bootstrap";
import {  AboutModel, ModelScene, 
    ModelSceneHandler, SceneInfo } from "@components/ModelScene";
import { Slider } from "@mui/material";

export interface SceneSettingsState extends AboutModel {
	sceneColor: string, lightIntensity: number,
    currentAnimation: string | null
}
export interface Settings extends SceneInfo {
    sceneColor: string, lightIntensity: number
}
export interface SceneSettingsProps {
    readonly modelData: Blob | undefined
}
export default class SceneSettings extends React.Component<SceneSettingsProps, SceneSettingsState> {
    private sceneRef = React.createRef<ModelSceneHandler>();
    public constructor(props: SceneSettingsProps) {
        super(props)
        this.state = { 
            animations: null, currentAnimation: null, 
            sceneColor: '#1c67d5', lightIntensity: 2, 
            memorySize: 0, trianglesCount: 0, verticesCount: 0,
        }
    }
    private sceneLoadedHandler = (info: AboutModel) => this.setState({...info})
    public async getSettings(): Promise<Settings> {
        const sceneInfo = await this.sceneRef.current!.getInfo();
        const { sceneColor, lightIntensity } = this.state;
        console.log(sceneInfo)
        return { sceneColor: sceneColor, lightIntensity: lightIntensity, ...sceneInfo }
    }
    public override render(): React.ReactNode {
        const { modelData } = this.props;
        const { sceneColor, lightIntensity, currentAnimation } = this.state;
        let { animations } = this.state;
        if (animations != null) animations = ['По умолчанию', ...animations]
        const modelScene = modelData == undefined ? <div></div> :
            <ModelScene modelData={modelData} onSceneLoaded={this.sceneLoadedHandler}
                sceneColor={sceneColor} 
                ref={this.sceneRef} lightIntensity={lightIntensity} currentAnimation={null}/>
        console.log(`anim: ${animations}`)
        return (
        <div>
            <Container fluid={'md'}>
                <Row className='justify-content-center mb-4'>
                    <Col md={12} style={fieldInfoStyle}>
                        <Form.Label>Превью модели:</Form.Label>
                        {modelScene}
                    </Col>
                </Row>
                <Row className='justify-content-center gy-2 mb-3'>
                    <Col md={6} lg={5} style={fieldInfoStyle}>
                        <Form.Label>Фоновая подсветка:</Form.Label>
                        <Slider value={lightIntensity} max={10} min={0} step={0.5} onChange={(_, value) => {
                            this.setState({lightIntensity: value as number})
                        }} />
                    </Col>
                    <Col xs={12} md={6} lg={5} style={{
                        display: 'flex',
                        flexFlow: 'row',
                        alignItems: 'center'
                    }}>
                        <Form.Label className="m-0 mx-4">Цвет сцены:</Form.Label>
                        <Form.Control type='color' defaultValue={sceneColor}
                            onChange={item => this.setState({sceneColor: item.target.value})}/>
                    </Col>
                </Row>
                <Row className='justify-content-center gy-2'>
                    <Col md={12} lg={10} style={fieldInfoStyle}>
                        <Form.Label>Список анимаций:</Form.Label>
                        <Form.Control as='select' style={infoFieldStyle} disabled={animations == null}
                            onChange={item => console.log(item)}>
                            { 
                            animations == null ? <option>Пусто</option>
                                : animations.map((item, index) => <option  key={index}>{item}</option>) 
                            }
                        </Form.Control>
                    </Col>
                </Row>
                <div className='my-4' style={{display: 'flex', justifyContent: 'center'}}>
                    <div style={{ border: '0px', backgroundColor: '#FFF',  height: '1px',  width: '100%'}}></div>
                </div>
                <Row className='justify-content-center gy-2'>
                    <Col md={12} lg={10} style={fieldInfoStyle}>
                        <Form.Label>Информация о сцене:</Form.Label>
                        <p style={{margin: '0px'}}>Занимаемая память: {this.state.memorySize.toFixed(2)} mb</p>
                        <p style={{margin: '0px'}}>Кол-во вершин: {this.state.verticesCount}</p>
                        <p style={{margin: '0px'}}>Кол-во полигонов: {this.state.trianglesCount}</p>
                    </Col>
                </Row>             
            </Container>
        </div>
        )
    }
}
const infoFieldStyle: React.CSSProperties = {
    backgroundColor: '#323232',
    color: '#FFF',
    resize: 'none'
}
const fieldInfoStyle: React.CSSProperties = {
    display: 'flex', 
    flexFlow: 'column', 
    alignItems: 'start'
}