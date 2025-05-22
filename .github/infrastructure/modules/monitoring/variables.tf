variable "app_name" {
  description = "Name prefix for resources"
  type        = string
}

variable "instance_id" {
  description = "EC2 instance ID to monitor"
  type        = string
}

variable "alarm_cpu_threshold" {
  description = "CPU threshold percentage"
  type        = number
  default     = 80
}

variable "sns_topic_arn" {
  description = "SNS topic ARN for CloudWatch alarms"
  type        = string
}
