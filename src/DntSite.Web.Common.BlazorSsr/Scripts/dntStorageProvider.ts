namespace DntBlazorSsr {
    export class DntStorageProvider {
        private static inMemoryStorage: { [key: string]: string } = {};

        static isSupported(): boolean {
            try {
                const testKey = "__DntStorageProvider__";
                localStorage.setItem(testKey, testKey);
                localStorage.removeItem(testKey);
                return true;
            } catch (e) {
                return false;
            }
        }

        static clear(): void {
            if (DntStorageProvider.isSupported()) {
                localStorage.clear();
            } else {
                DntStorageProvider.inMemoryStorage = {};
            }
        }

        static getItem(name: string): string | null {
            if (DntStorageProvider.isSupported()) {
                return localStorage.getItem(name);
            }
            if (DntStorageProvider.inMemoryStorage.hasOwnProperty(name)) {
                return DntStorageProvider.inMemoryStorage[name];
            }
            return null;
        }

        static key(index: number): string | null {
            if (DntStorageProvider.isSupported()) {
                return localStorage.key(index);
            } else {
                return Object.keys(DntStorageProvider.inMemoryStorage)[index] || null;
            }
        }

        static removeItem(name: string): void {
            if (DntStorageProvider.isSupported()) {
                localStorage.removeItem(name);
            } else {
                delete DntStorageProvider.inMemoryStorage[name];
            }
        }

        static setItem(name: string, value: string): void {
            if (DntStorageProvider.isSupported()) {
                localStorage.setItem(name, value);
            } else {
                DntStorageProvider.inMemoryStorage[name] = String(value); // not everyone uses TypeScript
            }
        }

        static count(): number {
            if (DntStorageProvider.isSupported()) {
                return localStorage.length;
            } else {
                return Object.keys(DntStorageProvider.inMemoryStorage).length;
            }
        }
    }
}
