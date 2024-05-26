import { useNavigate } from "react-router-dom";
import { headerRef } from "@core/App";

const apiKey = 'd98bfe8d-0127-4708-b537-08708f72698b';
const loginRedirectPath = '/login';

function headersFactory(formUsing: boolean = false): Headers {
    const headers = new Headers();
    const jwtToken = window.localStorage.getItem('jwtToken');
    
    headers.append('ApiKey', apiKey);
    if(jwtToken != null) headers.append('Authorization', `Bearer ${jwtToken}`);
    // if(formUsing) headers.append('Content-Type', `multipart/form-data`);
    return headers;
}
export type RequestInfo = {
    url: string;
    method?: 'GET' | 'POST' | 'DELETE' | 'PUT'
    body?: FormData
}
export type AuthorizationInfo = { login: string, password: string }

export type ApiAccessor = {
    accessor: <TData = any>(request: RequestInfo) => Promise<TData>,
    authorization: (request: AuthorizationInfo) => Promise<void>,
    logout: () => Promise<void>
}
export function useApiAccessor(): ApiAccessor {
    const navigator = useNavigate();
    return {
        accessor: async <TData = any>(request: RequestInfo) => {
            const { url, body, method } = request;
            const response = await window.fetch(url, {
                method: method == undefined ? 'GET' : method,
                body, headers: headersFactory(method == 'POST' || method == 'PUT'),
            })
            if (response.status == 401) {
                window.localStorage.removeItem('guid');
                window.localStorage.removeItem('jwtToken');
                navigator(loginRedirectPath)
            }
            await headerRef.current?.updateUser();
            if (response.status != 200) throw new Error(await response.text());
            return await (method == undefined || method == 'GET' ? response.json() 
                : response.text());
        },
        authorization: async ({login, password}: AuthorizationInfo) => {
            const url = `http://localhost:8080/modelsapp/auth/login?login=${login}&password=${password}`
            const response = await window.fetch(url, {
                method: 'GET',
                headers: { 'ApiKey': apiKey }
            })
            if(response.status != 200) throw new Error(await response.text());
            const authData = await response.json();
            window.localStorage.setItem('guid', authData.guid);
            window.localStorage.setItem('jwtToken', authData.jwtToken);
        },
        logout: async () => {
            window.localStorage.removeItem('guid');
            window.localStorage.removeItem('jwtToken');
            await headerRef.current?.updateUser();
            navigator(loginRedirectPath)
        }
    }
}