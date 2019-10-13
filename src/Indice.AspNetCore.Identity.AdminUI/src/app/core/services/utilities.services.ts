import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class UtilitiesService {
    public toInteger(value: any): number {
        return parseInt(`${value}`, 10);
    }

    public isNumber(value: any): value is number {
        return !isNaN(this.toInteger(value));
    }

    public padNumber(value: number): string {
        return this.isNumber(value) ? `0${value}`.slice(-2) : '';
    }

    public newGuid() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, (char) => {
            // tslint:disable-next-line: no-bitwise
            const random = Math.random() * 16 | 0;
            // tslint:disable-next-line: no-bitwise
            const value = char === 'x' ? random : (random & 0x3 | 0x8);
            return value.toString(16);
        });
    }

    public minutesToSeconds(minutes: number): number {
        return minutes * 60;
    }

    public hoursToSeconds(hours: number): number {
        return hours * 3600;
    }

    public daysToSeconds(days: number): number {
        return days * 86400;
    }

    public monthsToSeconds(months: number): number {
        return months * 2629744;
    }

    public yearsToSeconds(years: number): number {
        return years * 31556926;
    }

    public secondsToText(totalSeconds: number): string {
        const hours = Math.floor(totalSeconds / 3600);
        const minutes = Math.floor((totalSeconds - (hours * 3600)) / 60);
        const seconds = totalSeconds - (hours * 3600) - (minutes * 60);
        return `
            ${hours > 0 ? `${hours < 10 ? `0${hours}` : hours} hr${minutes > 0 || seconds > 0 ? ',' : ''}` : ''}
            ${minutes > 0 ? `${minutes < 10 ? `0${minutes}` : minutes} min${seconds > 0 ? ',' : ''}` : ''}
            ${seconds > 0 ? `${seconds < 10 ? `0${seconds}` : seconds} secs` : ''}
        `;
    }
}
