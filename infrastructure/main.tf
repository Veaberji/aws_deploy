terraform {
  backend "s3" {
    bucket       = "musicians-app-tf-state"
    key          = "terraform.tfstate"
    region       = "us-east-1"
    use_lockfile = true
    encrypt      = true
  }

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 4.16"
    }
  }

  required_version = ">= 1.2.0"
}

provider "aws" {
  region = var.region
}

locals {
  app_name = "musicians-app"
}

module "storage" {
  source = "./modules/storage"

  app_name    = local.app_name
  bucket_name = var.backend_bucket_name
}

module "network" {
  source = "./modules/network"

  app_name         = local.app_name
  api_port         = var.api_port
  ssh_ingress_cidr = var.ssh_ingress_cidr
}

module "compute" {
  source = "./modules/compute"

  app_name        = local.app_name
  ami             = var.ami
  volume_size     = var.volume_size
  instance_type   = var.instance_type
  security_groups = [module.network.sg_name]
  user_data       = file("${path.module}/scripts/user_data.sh")
}

module "monitoring" {
  source        = "./modules/monitoring"
  app_name      = local.app_name
  instance_id   = module.compute.instance_id
  sns_topic_arn = module.notifications.sns_topic_arn
}

module "notifications" {
  source              = "./modules/notifications"
  app_name            = local.app_name
  notification_emails = var.notification_emails
}
