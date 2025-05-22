variable "app_name" {
  description = "App name used in SNS topic naming"
  type        = string
}

variable "notification_emails" {
  description = "List of email addresses to subscribe"
  type        = list(string)
}
