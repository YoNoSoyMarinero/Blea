/**
 * @param func - function to be debounced
 * @param delay - debounce delay
 * @returns - returns debounced function of generic T
 */
export const debounce = <T extends (...args: any[]) => any>(func: T, delay: number) => {
  let timeoutId: NodeJS.Timeout | undefined;

  return (...args: Parameters<T>) => {
    return new Promise<ReturnType<T>>((resolve) => {
      if (timeoutId) {
        clearTimeout(timeoutId);
      }

      timeoutId = setTimeout(() => {
        resolve(func(...args));
      }, delay);
    });
  };
};

