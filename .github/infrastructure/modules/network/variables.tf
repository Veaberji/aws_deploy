variable "app_name" {
  description = "App name used for tags"
  type        = string
}

variable "ssh_ingress_cidr" {
  description = "CIDR blocks allowed to SSH"
  type        = list(string)
}

variable "api_port" {
  description = "Port used by app API"
  type        = number
}
