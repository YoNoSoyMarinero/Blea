/**
 * Custom hook for performing axios requests and returning states for tracking data, loading status and cancelation of request
 */
import { useState } from "react";
import axios, { AxiosError, AxiosRequestConfig, AxiosResponse } from 'axios';

const DEFAULT_FETCH_OPTIONS = {
    headers: {
        "Content-Type": "application/json"
    }
};

type UseFetchProps = {
  url: string;
  method: "get" | "post" | "put" | "delete"
};

type CommonRequest = {
  input?: { [index: string]: any };
  fetchOptions?: AxiosRequestConfig;
}

export function useFetch ({ url, method }: UseFetchProps) {
  const [isLoading, setIsLoading] = useState(false);
  const [data, setData] = useState(null);
  const [error, setError] = useState<AxiosError | null>(null);

  const commonRequest = async ({ input, fetchOptions = {}}: CommonRequest) => {
    setIsLoading(true);

    try{
      const response: AxiosResponse = await axios[method](url, input, {
        ...fetchOptions,
        ...DEFAULT_FETCH_OPTIONS
      });
      setData(await response.data);
    } catch (err: any) {
      console.log('original',err);
      setError(err);
    } finally {
      setIsLoading(false);
    }
  };

  return { isLoading, commonRequest, data, error };
};