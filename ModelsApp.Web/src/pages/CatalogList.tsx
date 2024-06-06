import CatalogCard, { CatalogCardProps } from '@components/cataloglist/CatalogCard';
import PageChanger from '@components/cataloglist/PageChanger';
import { useApiAccessor } from '@services/ApiAccess';
import React from 'react';
import { Button, Col, Container, Form, Row } from 'react-bootstrap';
import { Search } from 'react-bootstrap-icons';
import { useNavigate } from 'react-router-dom';

interface ModelInfoList {
    items: {
        guid: string, name: string;
        imageName: string, rating: number;
    }[],
    allCount: number;
}
const sortingTypes = ['По дате', 'По просмотрам', 'По рейтингу']
type CardInfo = Omit<CatalogCardProps, 'onCardClicked'>[];
export default function CatalogList(): React.JSX.Element {
    const itemsOnPage = 6;
    const { accessor } = useApiAccessor();
    const navigator = useNavigate();
    
    const [ sortingType, setSortingType ] = React.useState<number>(0);
    const [ page, setPage ] = React.useState<number>(1);
    const [ category, setCategory ] = React.useState<string | null>(null);
    const [ nameFilter, setNameFilter ] = React.useState<string | null>(null);
    const [ categoriesList, setCategoriesList ] = React.useState<string[]>([]);

    const [ catalogItems, setCatalogItems ] = React.useState<CardInfo>([]);
    const [ itemsCount, setItemsCount ] = React.useState<number>(0);
    const getModelsList = async () => {
        const url = new URL(`http://localhost:8080/modelsapp/models/getList`);
        url.searchParams.append('skip', (itemsOnPage * (page - 1)).toString());
        url.searchParams.append('take', itemsOnPage.toString());

        if(nameFilter != null) url.searchParams.append('nameFilter', nameFilter);
        if(category != null) url.searchParams.append('category', category);
        url.searchParams.append('sortingType', sortingType.toString());
        return await accessor<ModelInfoList>({url: url.toString()})
    }
    const searchClickHandler = React.useCallback(async () => {
        setPage(1);
        const models = await getModelsList();
        const items = models.items.map(item => ({
            name: item.name, rating: item.rating,
            imageUrl: item.imageName, guid: item.guid
        }))
        setItemsCount(models.allCount)
        setCatalogItems(items)
    }, [nameFilter, category, sortingType])
    const cardSelectHandler = (index: number) => {
        navigator(`/model/${catalogItems[index].guid}`)
    }
    React.useEffect(() => {
        accessor<string[]>({url: 'http://localhost:8080/modelsapp/models/categories'})
            .then(categories => setCategoriesList(['Все модели', ...categories]))
            .catch(error => console.log(error))
        getModelsList().then(models => {
                const items = models.items.map(item => ({
                    imageUrl: item.imageName, guid: item.guid,
                    name: item.name, rating: item.rating
                }))
                setItemsCount(models.allCount)
                setCatalogItems(items)
            })
            .catch(error => console.log(error))
    }, [page])
    return (
    <div style={{width: '100%'}}>
        <h1 style={{margin: '10px 0px 30px'}}>Каталог 3Д моделей</h1>
        <Container fluid={'md'}>
            <Row className='justify-content-center mb-5 gy-3'>
                <Col xs={10} md={4} lg={5} style={fieldInfoStyle}>
                    <Form.Label>Поиск по названию:</Form.Label>
                    <Form.Control style={infoFieldStyle}
                        onChange={item =>  setNameFilter(item.target.value)} placeholder='Введите название'/>
    
                </Col>
                <Col xs={10} md={3} lg={2} style={fieldInfoStyle}>
                    <Form.Label>Категория:</Form.Label>
                    <Form.Control as='select' style={infoFieldStyle} value={category == null ? 'Все модели' : category}
                        onChange={item => {
                            const current = item.target.value;
                            setCategory(current == 'Все модели' ? null : current)
                        }}>
                        { 
                        categoriesList.map((item, index) => <option key={index}>{item}</option>) 
                        }
                    </Form.Control>
                </Col>
                <Col xs={10} md={3} lg={2} style={fieldInfoStyle}>
                    <Form.Label>Сортировать:</Form.Label>
                    <Form.Control as='select' style={infoFieldStyle} value={sortingTypes[sortingType]}
                        onChange={item => {
                            setSortingType(sortingTypes.indexOf(item.target.value))
                        }}>
                        { 
                        sortingTypes.map((item, index) => <option key={index}>{item}</option>) 
                        }
                    </Form.Control>
                </Col>
                <Col xs={10} md={1} lg={1} style={{display: 'flex', alignItems: 'end'}}>
                    <Button onClick={searchClickHandler}>
                        <Search/>
                    </Button>
                </Col>
            </Row>
            <Row className='justify-content-center justify-content-sm-start gy-4 gx-4 mb-4'>
            { 
                catalogItems.map((item, index) => {
                    const { imageUrl, name, rating } = item;
                    return (
                    <Col key={`col-${index}`} xs={10} sm={6} md={4} lg={3}>
                        <CatalogCard key={index} imageUrl={imageUrl} name={name} rating={rating} 
                            onCardClicked={() => cardSelectHandler(index)}/>
                    </Col>
                    )
                }) 
            }
            </Row>
            <Row className='justify-content-center'>
                <Col md={12} style={{display: 'flex', justifyContent: 'center'}}>
                    <PageChanger pageCount={Math.ceil(itemsCount/itemsOnPage)} onPageChange={(current) => {
                        setPage(current)
                    }}/>
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
const fieldInfoStyle: React.CSSProperties = {
    display: 'flex', 
    flexFlow: 'column', 
    alignItems: 'start'
}