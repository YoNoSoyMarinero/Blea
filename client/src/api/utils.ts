import axios, { AxiosError, AxiosRequestConfig, AxiosResponse } from 'axios';
import { AxiosResponseResult, SendRequestResult } from '../types/globalTypes';

const DEFAULT_FETCH_OPTIONS: AxiosRequestConfig = {
    headers: {
        "Content-Type": "application/json",
        withCredentials: true,
    }
};

type RequestProps = {
    url: string;
    method: "get" | "post" | "put" | "delete";
    input?: { [index: string]: unknown };
    fetchOptions?: AxiosRequestConfig;
};

export const AxiosRequest =  ({
    url,
    method,
    input,
    fetchOptions
}: RequestProps): AxiosResponseResult => {
    let data: any = null;
    let error: AxiosError | null = null;
    let abortController = new AbortController();

    const sendRequest = async (): Promise<SendRequestResult> => {
        try {
            const response: AxiosResponse = await axios[method](url, input, {
                ...fetchOptions,
                ...DEFAULT_FETCH_OPTIONS,
                signal: abortController.signal
            });
    
            data = response.data;
        } catch (err) {
            if (axios.isCancel(err)) {
                console.log('canceled');
            }
            error = err;
        }

        return { data, error };
    }
    
    const cancelRequest = () => {
        abortController.abort();
    }

    return { sendRequest, cancelRequest};
}
