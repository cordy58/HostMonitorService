export interface PingResult {
  host: string;
  roundtripTime: number;
  tcpPortOpen: boolean;
  success: boolean;
  errorMessage?: string;
  checkedAt: Date;
}