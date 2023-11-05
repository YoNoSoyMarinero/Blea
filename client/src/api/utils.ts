import axios, { AxiosError, AxiosRequestConfig, AxiosResponse } from 'axios';
import { AxiosResponseResult } from '../types/globalTypes';

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

export const AxiosRequest = async ({
    url,
    method,
    input,
    fetchOptions
}: RequestProps): Promise<AxiosResponseResult> => {
    let data: any = null;
    let error: AxiosError | null = null;

    try {
        const response: AxiosResponse = await axios[method](url, input, {
            ...fetchOptions,
            ...DEFAULT_FETCH_OPTIONS,
        });

        data = response.data;
    } catch (err) {
        error = err;
    }

    return { data, error };
}
