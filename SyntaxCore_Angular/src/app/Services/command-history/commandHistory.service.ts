import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})

export class CommandHistoryService {
    private history: string [] = [];
    private currentIndex: number = 0;

    public addCommand(command: string): void {
        this.history.push(command);
        this.currentIndex = this.history.length; 
    }
    public getHistory(): string[] { 
        return this.history;
    }
    public getPreviousCommand(): string | null {
        if (this.currentIndex > 0) {
            this.currentIndex--;
            return this.history[this.currentIndex] ?? '';
        }   
        return null;
    }
    //[1 2' 3]
    public getNextCommand(): string | null {
        if (this.currentIndex < this.history.length - 1) {
            this.currentIndex++;
            return this.history[this.currentIndex] ?? '';
        } else if (this.currentIndex === this.history.length - 1) {
            this.currentIndex++;
            return '';
        }
        return null;
    }
}