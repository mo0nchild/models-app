import { RequestInfo } from "@services/ApiAccess";
import React from "react";
import { Col, Form, Row, Container } from "react-bootstrap";

export interface ModelCreateInfo {
    name?: string | undefined,
    description?: string | undefined,
    category?: string | undefined,
    file?: Blob | undefined,
}
export interface ModelInfoState extends ModelCreateInfo {
    categories: string[]
}
export interface ModelInfoProps extends ModelCreateInfo {
    accessor: <TData = any>(request: RequestInfo) => Promise<TData>
}
export default class ModelInfo extends React.Component<ModelInfoProps, ModelInfoState> {
    private static modelExtension = 'glb';
    public constructor(props: ModelInfoProps) {
        super(props);
        this.state = {...props, categories: []}
    }
    public override componentDidMount(): void {
        const { accessor } = this.props;
        accessor<string[]>({url: 'http://localhost:8080/modelsapp/models/categories'})
            .then(item => this.setState({categories: item, category: item[0]}))
            .catch(error => console.log((error as Error).message));
    }
    public getState(): ModelCreateInfo { return { ...this.state }; }
    private fileLoadHandler = (filename: string, data: Blob) => {
        const segment = filename.split('.');
        if(segment.length <= 0 || segment[segment.length - 1] != ModelInfo.modelExtension) return;
        this.setState({file: data});
    }
    public override render(): React.ReactNode {
        const { categories } = this.state;
        const { category, name, description } = this.props
        return (
        <div>
            <Container fluid={'md'}>
                <Row className='justify-content-center gy-2 mb-2'>
                    <Col md={6} lg={5} style={fieldInfoStyle}>
                        <Form.Label>Название:</Form.Label>
                        <Form.Control onChange={event => this.setState({ name: event.target.value })} 
                            type='text' defaultValue={name} 
                            style={infoFieldStyle} placeholder="Название модели" maxLength={20}/>
                    </Col>
                    <Col md={6} lg={5} style={fieldInfoStyle}>
                        <Form.Label>Категория:</Form.Label>
                        <Form.Control as='select' style={infoFieldStyle} value={category}
                            onChange={item => this.setState({category: item.target.value})}>
                            { 
                            categories.map((item, index) => (<option key={index}>{item}</option>)) 
                            }
                        </Form.Control>
                    </Col>
                </Row>
                <Row className='justify-content-center gy-2 mb-2'>
                    <Col md={12} lg={10} style={fieldInfoStyle}>
                        <Form.Label>Описание:</Form.Label>
                        <Form.Control as='textarea' type="text" placeholder="Описание для публикации"
                            rows={3} style={infoFieldStyle} maxLength={50} defaultValue={description}
                            onChange={event => this.setState({description: event.target.value})}/>
                    </Col>
                </Row>
                <Row className='justify-content-center gy-2 mb-2'>
                    <Col md={12} lg={10} style={fieldInfoStyle}>
                        <Form.Label>Файл для загрузки:</Form.Label>
                        <Form.Control type='file' accept='.glb' style={infoFieldStyle}
                            onChange={item => {
                                const { value, files } = item.target as HTMLInputElement;
                                if (files == null || files.length <= 0) return;
                                this.fileLoadHandler(value, files[0])
                            }}/>
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

