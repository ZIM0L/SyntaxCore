import { Component, inject, ElementRef, ViewChild } from '@angular/core';
import { CommandHistoryService } from '../../Services/command-history/commandHistory.service';
import { DeviceInfoService } from '../../Services/device-info/deviceInfo.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'main-console',
  standalone: true,
  templateUrl: './main-console.component.html',
  styleUrl: '../../../sass/_console.css',
  imports: [FormsModule],
})
export class MainConsoleComponent {
@ViewChild('cmd') cmd!: ElementRef;
@ViewChild('terminal') terminal!: ElementRef;

prompt = '';
deviceInfo = '';

commandService = inject(CommandHistoryService);
deviceInfoService = inject(DeviceInfoService);

isLoading = true;
constructor() { }

onKeyPress() {
  const terminal = this.terminal.nativeElement;

  const isAtTop = terminal.scrollTop === 0;

  if (isAtTop) {
    setTimeout(() => {
      terminal.scrollTop = terminal.scrollHeight;
      this.cmd.nativeElement.focus(); 
    }, 0);
  }
}

submitPrompt() {
  if (this.prompt.trim() === '') {
    return;
  }
  this.addDiv();
}

addDiv() {
  this.commandService.addCommand(this.prompt);
}
getPreviusCommand() {
  this.prompt = this.commandService.getPreviousCommand() ?? '';
}
getNextCommand() {
  this.prompt = this.commandService.getNextCommand() ?? '';
}

  ngAfterViewInit() {
    setTimeout(() => this.cmd.nativeElement.focus(), 0);
  }

ngOnInit() {
  this.deviceInfo = this.deviceInfoService.getDeviceInfo();
  this.isLoading = false;
}

}
